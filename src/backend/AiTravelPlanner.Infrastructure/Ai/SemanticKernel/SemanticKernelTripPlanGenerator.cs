using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.Prompting;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;
using AiTravelPlanner.Infrastructure.Ai.ExtensionsAi;
using AiTravelPlanner.Infrastructure.Ai.Models;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

namespace AiTravelPlanner.Infrastructure.Ai.SemanticKernel;

public sealed class SemanticKernelTripPlanGenerator : ITripPlanGenerator
{
    private readonly Kernel _kernel;
    private readonly ITripGenerationPromptBuilder _promptBuilder;
    private readonly ExtensionsAiOptions _options;

    public SemanticKernelTripPlanGenerator(Kernel kernel, ITripGenerationPromptBuilder promptBuilder, IOptions<ExtensionsAiOptions> options)
    {
        _kernel = kernel;
        _promptBuilder = promptBuilder;
        _options = options.Value;
    }

    public async Task<Plan> GenerateAsync(GenerateTripCommand command, CancellationToken cancellationToken, string? additionalInstruction = null)
    {
        var systemPrompt = _promptBuilder.BuildSystemPrompt();
        var userPrompt = _promptBuilder.Build(command, additionalInstruction);

        var prompt = $$"""
            {{systemPrompt}}

            User request:
            {{userPrompt}}
        """;

        var result = await _kernel.InvokePromptAsync(prompt, cancellationToken: cancellationToken);

        var generatedPlan = GeneratedTripPlanParser.Parse(result.ToString(), "SemanticKernel");
        var aiMetadata = new AiGenerationMetadata(
            Provider: "SemanticKernel",
            Model: _options.Model,
            PromptTokens: null,
            CompletionTokens: null,
            TotalTokens: null);

        return generatedPlan.ToPlan(command, aiMetadata);
    }
}