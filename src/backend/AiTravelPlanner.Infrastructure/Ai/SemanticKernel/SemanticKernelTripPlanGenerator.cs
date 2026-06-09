using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.Prompting;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;
using AiTravelPlanner.Infrastructure.Ai.Chat;
using AiTravelPlanner.Infrastructure.Ai.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AiTravelPlanner.Infrastructure.Ai.SemanticKernel;

public sealed class SemanticKernelTripPlanGenerator : ITripPlanGenerator
{
    private readonly Kernel _kernel;
    private readonly ITripGenerationPromptBuilder _promptBuilder;
    private readonly GitHubModelsChatOptions _options;

    private const string GenerateTripPromptTemplate = """
    {{$systemPrompt}}

    User request:
    {{$userPrompt}}

    Use the available tools when useful for exact calculations
    """;

    private readonly KernelFunction _generateTripFunction;

    public SemanticKernelTripPlanGenerator(Kernel kernel, ITripGenerationPromptBuilder promptBuilder, IOptions<GitHubModelsChatOptions> options)
    {
        _kernel = kernel;
        _promptBuilder = promptBuilder;
        _options = options.Value;

        var executionSettings = new OpenAIPromptExecutionSettings
        {
            Temperature = (double)_options.Temperature,
            MaxTokens = _options.MaxTokens,
            ResponseFormat = "json_object",
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

        var chatResponse = result.GetValue<ChatResponse>() ?? throw new InvalidOperationException("Semantic Kernel returned no chat response.");

        var generatedPlan = GeneratedTripPlanParser.Parse(chatResponse.Text, "SemanticKernel");

        var aiMetadata = new AiGenerationMetadata(
            Provider: "SemanticKernel",
            Model: chatResponse.ModelId ?? _options.Model,
            PromptTokens: ToInt(chatResponse.Usage?.InputTokenCount),
            CompletionTokens: ToInt(chatResponse.Usage?.OutputTokenCount),
            TotalTokens: ToInt(chatResponse.Usage?.TotalTokenCount));

        return generatedPlan.ToPlan(command, aiMetadata);
    }

    private static int? ToInt(long? value)
    {
        return value is null
            ? null
            : checked((int)value.Value);
    }
}
