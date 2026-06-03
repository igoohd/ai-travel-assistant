using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.Prompting;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;
using Microsoft.Extensions.AI;

namespace AiTravelPlanner.Infrastructure.Ai.ExtensionsAi;

public sealed class ExtensionsAiTripPlanGenerator : ITripPlanGenerator
{
    private readonly IChatClient _chatClient;
    private readonly ITripGenerationPromptBuilder _promptBuilder;

    public ExtensionsAiTripPlanGenerator(IChatClient chatClient, ITripGenerationPromptBuilder promptBuilder)
    {
        _chatClient = chatClient;
        _promptBuilder = promptBuilder;
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

        throw new NotImplementedException(
            $"Extensions AI returned: {response.Text}");
    }
}