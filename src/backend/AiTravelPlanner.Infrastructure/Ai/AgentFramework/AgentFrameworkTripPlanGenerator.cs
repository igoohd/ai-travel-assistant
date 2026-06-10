using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.Prompting;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;
using AiTravelPlanner.Infrastructure.Ai.Chat;
using AiTravelPlanner.Infrastructure.Ai.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Options;

namespace AiTravelPlanner.Infrastructure.Ai.AgentFramework;

public sealed class AgentFrameworkTripPlanGenerator : ITripPlanGenerator
{
    private readonly AIAgent _agent;
    private readonly ITripGenerationPromptBuilder _promptBuilder;
    private readonly GitHubModelsChatOptions _options;

    public AgentFrameworkTripPlanGenerator(
        AIAgent agent,
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

        var response = await _agent.RunAsync(
            prompt,
            cancellationToken: cancellationToken);

        var generatedPlan = GeneratedTripPlanParser.Parse(
            response.Text,
            "AgentFramework");

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