using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.Prompting;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;
using AiTravelPlanner.Infrastructure.Ai.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;

namespace AiTravelPlanner.Infrastructure.Ai.ExtensionsAi;

public sealed class ExtensionsAiTripPlanGenerator : ITripPlanGenerator
{
    private readonly IChatClient _chatClient;
    private readonly ITripGenerationPromptBuilder _promptBuilder;
    private readonly ExtensionsAiOptions _options;

    public ExtensionsAiTripPlanGenerator(IChatClient chatClient, ITripGenerationPromptBuilder promptBuilder, IOptions<ExtensionsAiOptions> options)
    {
        _chatClient = chatClient;
        _promptBuilder = promptBuilder;
        _options = options.Value;
    }

    public async Task<Plan> GenerateAsync(GenerateTripCommand command, CancellationToken cancellationToken, string? additionalInstruction = null)
    {
        var prompt = _promptBuilder.Build(command, additionalInstruction);
        var systemPrompt = _promptBuilder.BuildSystemPrompt();

        var response = await _chatClient.GetResponseAsync(
            [
                new ChatMessage(ChatRole.System, systemPrompt),
                new ChatMessage(ChatRole.User, prompt)
            ],
            cancellationToken: cancellationToken);

        var generatedPlan = GeneratedTripPlanParser.Parse(response.Text, "ExtensionsAI");

        var aiMetadata = new AiGenerationMetadata(
            Provider: "ExtensionsAI",
            Model: response.ModelId ?? _options.Model,
            PromptTokens: ToInt(response.Usage?.InputTokenCount),
            CompletionTokens: ToInt(response.Usage?.OutputTokenCount),
            TotalTokens: ToInt(response.Usage?.TotalTokenCount)
        );

        return generatedPlan.ToPlan(command, aiMetadata);
    }

    private static int? ToInt(long? value)
    {
        return value is null ? null : checked((int)value.Value);
    }
}