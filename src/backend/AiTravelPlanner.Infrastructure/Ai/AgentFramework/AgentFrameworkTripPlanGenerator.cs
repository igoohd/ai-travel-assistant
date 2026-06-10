using System.Text.Json;
using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.Prompting;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;
using AiTravelPlanner.Infrastructure.Ai.Chat;
using AiTravelPlanner.Infrastructure.Ai.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AiTravelPlanner.Infrastructure.Ai.AgentFramework;

public sealed class AgentFrameworkTripPlanGenerator : ITripPlanGenerator
{
    private readonly ChatClientAgent _agent;
    private readonly AgentFrameworkTripPlanReviewer _reviewer;
    private readonly ITripGenerationPromptBuilder _promptBuilder;
    private readonly GitHubModelsChatOptions _options;
    private readonly ILogger<AgentFrameworkTripPlanGenerator> _logger;

    public AgentFrameworkTripPlanGenerator(
        [FromKeyedServices(AgentKeys.Planner)] ChatClientAgent agent,
        AgentFrameworkTripPlanReviewer reviewer,
        ITripGenerationPromptBuilder promptBuilder,
        ILogger<AgentFrameworkTripPlanGenerator> logger,
        IOptions<GitHubModelsChatOptions> options)
    {
        _agent = agent;
        _reviewer = reviewer;
        _promptBuilder = promptBuilder;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<Plan> GenerateAsync(
        GenerateTripCommand command,
        CancellationToken cancellationToken,
        string? additionalInstruction = null)
    {
        var prompt = _promptBuilder.Build(
            command,
            additionalInstruction);

        var runOptions = new ChatClientAgentRunOptions(
            new ChatOptions
            {
                Temperature = (float)_options.Temperature,
                MaxOutputTokens = _options.MaxTokens
            });

        var response = await _agent.RunAsync<GeneratedTripPlan>(
            prompt,
            options: runOptions,
            cancellationToken: cancellationToken);

        var generatedPlan = response.Result;

        var plan = generatedPlan.ToPlan(
            command,
            CreateMetadata(response.Usage));

        var review = await _reviewer.ReviewAsync(
            plan,
            command,
            cancellationToken);

        var validationIssues = review.Issues;

        _logger.LogInformation(
            "ValidatorAgent reviewed trip. Findings: {FindingCount}. Codes: {Codes}",
            validationIssues.Count,
            string.Join(", ", validationIssues.Select(issue => issue.Code)));

        if (validationIssues.Count == 0)
        {
            return generatedPlan.ToPlan(
                command,
                CreateMetadata(response.Usage, review.Usage));
        }

        var findings = string.Join(
            Environment.NewLine,
            validationIssues.Select(issue =>
                $"- {issue.Code}: {issue.Message}"));

        var revisionInstruction = $"""
        Revise the previous itinerary using this validator feedback:

        {findings}

        Previous itinerary:
        {JsonSerializer.Serialize(generatedPlan)}

        Return the complete revised itinerary.
        """;

        var revisedPrompt = _promptBuilder.Build(
            command,
            revisionInstruction);

        var revisedResponse = await _agent.RunAsync<GeneratedTripPlan>(
            revisedPrompt,
            options: runOptions,
            cancellationToken: cancellationToken);

        return revisedResponse.Result.ToPlan(
            command,
            CreateMetadata(
                response.Usage,
                review.Usage,
                revisedResponse.Usage));
    }

    private AiGenerationMetadata CreateMetadata(params UsageDetails?[] usages)
    {
        return new AiGenerationMetadata(
            Provider: "AgentFramework",
            Model: _options.Model,
            PromptTokens: SumTokens(
                usages.Select(usage => usage?.InputTokenCount)),
            CompletionTokens: SumTokens(
                usages.Select(usage => usage?.OutputTokenCount)),
            TotalTokens: SumTokens(
                usages.Select(usage => usage?.TotalTokenCount)));
    }

    private static int? SumTokens(IEnumerable<long?> values)
    {
        var availableValues = values
            .Where(value => value.HasValue)
            .Select(value => value!.Value)
            .ToArray();

        return availableValues.Length == 0
            ? null
            : checked((int)availableValues.Sum());
    }
}
