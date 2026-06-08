using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.Prompting;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;
using AiTravelPlanner.Infrastructure.Ai.Models;
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

        var generatedPlan = GeneratedTripPlanParser.Parse(completion.Content, "GitHubModels");

        _logger.LogInformation(
            "GitHub Models generated trip plan JSON with {DayCount} days.",
            generatedPlan.Days.Count
        );

        var aiMetadata = new AiGenerationMetadata(
            Provider: "GitHubModels",
            Model: _options.Model,
            PromptTokens: completion.Usage?.PromptTokens,
            CompletionTokens: completion.Usage?.CompletionTokens,
            TotalTokens: completion.Usage?.TotalTokens
        );

        return generatedPlan.ToPlan(command, aiMetadata);
    }
}
