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
        var prompt = $"""
            Create a short travel plan overview.

            Destination: {command.Destination}
            Days: {command.NumberOfDays}
            Budget: {command.Budget} {command.Currency}
            Interests: {string.Join(", ", command.Interests)}

            Return only a concise overview paragraph.
            """;

        var overview = await _client.CompleteChatAsync(
            [
                new GitHubModelsMessage(
                    Role: "system",
                    Content: "You are a helpful travel planning assistant."),
                new GitHubModelsMessage(
                    Role: "user",
                    Content: prompt)
            ]);

        var currency = new CurrencyCode(command.Currency);

        return new Plan(
            Destination: command.Destination,
            NumberOfDays: command.NumberOfDays,
            Overview: overview,
            Days: [],
            Budget: new BudgetEstimate(
                Hotel: 0,
                Transportation: 0,
                Food: 0,
                Activities: 0,
                Total: command.Budget,
                Currency: currency,
                Category: "unknown"),
            Highlights: [],
            TravelTips: []);
    }
}
