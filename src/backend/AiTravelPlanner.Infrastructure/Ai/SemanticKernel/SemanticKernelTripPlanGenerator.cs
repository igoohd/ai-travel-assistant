using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.Prompting;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;
using AiTravelPlanner.Infrastructure.Ai.ExtensionsAi;
using AiTravelPlanner.Infrastructure.Ai.Models;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AiTravelPlanner.Infrastructure.Ai.SemanticKernel;

public sealed class SemanticKernelTripPlanGenerator : ITripPlanGenerator
{
    private readonly Kernel _kernel;
    private readonly ITripGenerationPromptBuilder _promptBuilder;
    private readonly ExtensionsAiOptions _options;

    private const string GenerateTripPromptTemplate = """
    {{$systemPrompt}}

    User request:
    {{$userPrompt}}

    Use the available tools when useful for exact calculations
    """;

    private readonly KernelFunction _generateTripFunction;

    public SemanticKernelTripPlanGenerator(Kernel kernel, ITripGenerationPromptBuilder promptBuilder, IOptions<ExtensionsAiOptions> options)
    {
        _kernel = kernel;
        _promptBuilder = promptBuilder;
        _options = options.Value;

        var executionSettings = new OpenAIPromptExecutionSettings
        {
            Temperature = (double)_options.Temperature,
            MaxTokens = _options.MaxTokens,
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        _generateTripFunction = _kernel.CreateFunctionFromPrompt(
            GenerateTripPromptTemplate,
            executionSettings,
            functionName: "GenerateTrip");
    }

    public async Task<Plan> GenerateAsync(GenerateTripCommand command, CancellationToken cancellationToken, string? additionalInstruction = null)
    {
        var systemPrompt = _promptBuilder.BuildSystemPrompt();
        var userPrompt = _promptBuilder.Build(command, additionalInstruction);

        var arguments = new KernelArguments
        {
            ["systemPrompt"] = systemPrompt,
            ["userPrompt"] = userPrompt
        };

        var result = await _kernel.InvokeAsync(
            _generateTripFunction,
            arguments,
            cancellationToken: cancellationToken);


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