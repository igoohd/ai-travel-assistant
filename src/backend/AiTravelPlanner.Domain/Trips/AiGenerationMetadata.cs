namespace AiTravelPlanner.Domain.Trips;

public sealed record AiGenerationMetadata(
    string Provider,
    string Model,
    int? PromptTokens,
    int? CompletionTokens,
    int? TotalTokens);
