using System.Text.Json;
using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Application.Trips.Services;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Infrastructure.Ai.GitHubModels;

public sealed class GitHubModelsTripPlanGenerator : ITripPlanGenerator
{
    private readonly IGitHubModelsClient _client;

    public GitHubModelsTripPlanGenerator(IGitHubModelsClient client)
    {
        _client = client;
    }

    public async Task<Plan> GenerateAsync(GenerateTripCommand command)
    {
        var prompt = @"Return only valid JSON matching this shape:
        {
            ""overview"": ""string"",
            ""days"": [
                {
                ""dayNumber"": 1,
                ""title"": ""string"",
                ""description"": ""string"",
                ""activities"": [
                    {
                    ""timeOfDay"": ""Morning"",
                    ""title"": ""string"",
                    ""description"": ""string"",
                    ""estimatedCost"": 50
                    }
                ],
                ""restaurants"": [
                    {
                    ""name"": ""string"",
                    ""cuisine"": ""string"",
                    ""notes"": ""string"",
                    ""estimatedCost"": 40
                    }
                ]
                }
            ],
            ""highlights"": [""string""],
            ""travelTips"": [""string""]
        }";

        var aiContent = await _client.CompleteChatAsync(
            [
                new GitHubModelsMessage(
                    Role: "system",
                    Content: "You are a helpful travel planning assistant."),
                new GitHubModelsMessage(
                    Role: "user",
                    Content: prompt)
            ]);

        var jsonContent = ExtractJsonObject(aiContent);

        var generatedPlan = JsonSerializer.Deserialize<GeneratedTripPlan>(
            jsonContent,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        if (generatedPlan is null)
        {
            throw new InvalidOperationException("GitHub Models returned an invalid trip plan.");
        }

        var currency = new CurrencyCode(command.Currency);

        return new Plan(
            Destination: command.Destination,
            NumberOfDays: command.NumberOfDays,
            Overview: generatedPlan.Overview,
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
                            EstimatedCost: activity.EstimatedCost))
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
                Hotel: 0,
                Transportation: 0,
                Food: 0,
                Activities: 0,
                Total: command.Budget,
                Currency: currency,
                Category: "unknown"),
            Highlights: generatedPlan.Highlights,
            TravelTips: generatedPlan.TravelTips);
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

}
