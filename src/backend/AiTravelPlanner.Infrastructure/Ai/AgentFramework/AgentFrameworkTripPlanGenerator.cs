using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.Prompting;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;
using AiTravelPlanner.Infrastructure.Ai.Chat;
using AiTravelPlanner.Infrastructure.Ai.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;

namespace AiTravelPlanner.Infrastructure.Ai.AgentFramework;

public sealed class AgentFrameworkTripPlanGenerator : ITripPlanGenerator
{
    private readonly ChatClientAgent _agent;
    private readonly ITripGenerationPromptBuilder _promptBuilder;
    private readonly GitHubModelsChatOptions _options;

    public AgentFrameworkTripPlanGenerator(
        ChatClientAgent agent,
        ITripGenerationPromptBuilder promptBuilder,
        IOptions<GitHubModelsChatOptions> options)
    {
        _agent = agent;
        _promptBuilder = promptBuilder;
        _options = options.Value;
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
                MaxOutputTokens = _options.MaxTokens,
            }
        );

        var response = await _agent.RunAsync<GeneratedTripPlan>(
            prompt,
            options: runOptions,
            cancellationToken: cancellationToken);

        var generatedPlan = response.Result;

        var aiMetadata = new AiGenerationMetadata(
            Provider: "AgentFramework",
            Model: _options.Model,
            PromptTokens: ToInt(response.Usage?.InputTokenCount),
            CompletionTokens: ToInt(response.Usage?.OutputTokenCount),
            TotalTokens: ToInt(response.Usage?.TotalTokenCount));

        return generatedPlan.ToPlan(command, aiMetadata);
    }

    private static int? ToInt(long? value)
    {
        return value is null
            ? null
            : checked((int)value.Value);
    }
}