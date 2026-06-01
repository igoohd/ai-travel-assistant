using System.Text.Json;
using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.Prompting;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AiTravelPlanner.Infrastructure.Ai.GitHubModels;

public sealed class GitHubModelsTripPlanGenerator : ITripPlanGenerator
{
    private readonly IGitHubModelsClient _client;
    private readonly GitHubModelsOptions _options;
    private readonly ILogger<GitHubModelsTripPlanGenerator> _logger;
    private readonly ITripGenerationPromptBuilder _promptBuilder;

    public GitHubModelsTripPlanGenerator(
        IGitHubModelsClient client,
        ILogger<GitHubModelsTripPlanGenerator> logger,
        IOptions<GitHubModelsOptions> options,
        ITripGenerationPromptBuilder promptBuilder)
    {
        _client = client;
        _logger = logger;
        _options = options.Value;
        _promptBuilder = promptBuilder;
    }

    public async Task<Plan> GenerateAsync(
        GenerateTripCommand command,
        CancellationToken cancellationToken,
        string? additionalInstruction = null)
    {

        var prompt = _promptBuilder.Build(command, additionalInstruction);
        var systemPrompt = _promptBuilder.BuildSystemPrompt();
        var completion = await _client.CompleteChatAsync(
            [
                new GitHubModelsMessage(
                    Role: "system",
                    Content: systemPrompt),
                new GitHubModelsMessage(
                    Role: "user",
                    Content: prompt)
            ],
            cancellationToken);

        var jsonContent = ExtractJsonObject(completion.Content);

        GeneratedTripPlan? generatedPlan;
        try
        {
            generatedPlan = JsonSerializer.Deserialize<GeneratedTripPlan>(
                jsonContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
        catch (JsonException exception)
        {
            throw new InvalidOperationException(
                "GitHub Models returned a response that could not be parsed as a trip plan JSON object.",
                exception);
        }

        if (generatedPlan is null)
        {
            throw new InvalidOperationException("GitHub Models returned an invalid trip plan.");
        }

        _logger.LogInformation(
            "GitHub Models generated trip plan JSON with {DayCount} days.",
            generatedPlan.Days.Count
        );

        var currency = new CurrencyCode(command.Currency);

        var activityTotal = generatedPlan.Days
            .SelectMany(day => day.Activities)
            .Sum(activity => activity.EstimatedCost);

        var foodTotal = generatedPlan.Days
            .SelectMany(day => day.Restaurants)
            .Sum(restaurant => restaurant.EstimatedCost);

        var hotelTotal = Math.Round(command.Budget * 0.40m, 2);
        var transportationTotal = Math.Round(command.Budget * 0.15m, 2);

        var estimatedTotal = hotelTotal + transportationTotal + foodTotal + activityTotal;

        return new Plan(
            Id: Guid.NewGuid(),
            CreatedAt: DateTimeOffset.UtcNow,
            AiMetadata: new AiGenerationMetadata(
                Provider: "GitHubModels",
                Model: _options.Model,
                PromptTokens: completion.Usage?.PromptTokens,
                CompletionTokens: completion.Usage?.CompletionTokens,
                TotalTokens: completion.Usage?.TotalTokens
                ),
            Destination: command.Destination,
            NumberOfDays: command.NumberOfDays,
            Days: generatedPlan.Days
                .Select(day => new Day(
                    DayNumber: day.DayNumber,
                    Title: day.Title,
                    Description: day.Description,
                    Activities: day.Activities
                        .Select(activity => new Activity(
                            TimeOfDay: activity.TimeOfDay,
                            Title: activity.Title,
                            Description: activity.Description,
                            EstimatedCost: activity.EstimatedCost,
                            DurationHours: activity.DurationHours,
                            TransitMinutesFromPrevious: activity.TransitMinutesFromPrevious))
                        .ToArray(),
                    Restaurants: day.Restaurants
                        .Select(restaurant => new RestaurantSuggestion(
                            Name: restaurant.Name,
                            Cuisine: restaurant.Cuisine,
                            Notes: restaurant.Notes,
                            EstimatedCost: restaurant.EstimatedCost))
                        .ToArray()))
                .ToArray(),
            Budget: new BudgetEstimate(
                Hotel: hotelTotal,
                Transportation: transportationTotal,
                Food: foodTotal,
                Activities: activityTotal,
                Total: estimatedTotal,
                Currency: currency,
                Category: ClassifyBudget(estimatedTotal, command.NumberOfDays)),
            Summary: new Summary(
                Highlights: generatedPlan.Highlights,
                TravelTips: generatedPlan.TravelTips,
                Overview: generatedPlan.Overview
            )
        );
    }

    private static string ExtractJsonObject(string content)
    {
        var trimmedContent = content.Trim();

        if (trimmedContent.StartsWith("```"))
        {
            var firstNewLineIndex = trimmedContent.IndexOf('\n');
            var lastFenceIndex = trimmedContent.LastIndexOf("```", StringComparison.Ordinal);

            if (firstNewLineIndex >= 0 && lastFenceIndex > firstNewLineIndex)
            {
                trimmedContent = trimmedContent[
                    (firstNewLineIndex + 1)..lastFenceIndex
                ].Trim();
            }
        }

        return trimmedContent;
    }

    private static string ClassifyBudget(decimal total, int numberOfDays)
    {
        var dailyBudget = total / numberOfDays;

        return dailyBudget switch
        {
            < 100 => "budget",
            < 300 => "mid-range",
            _ => "luxury"
        };
    }

}
