namespace AiTravelPlanner.Infrastructure.Ai.GitHubModels;

public sealed class GitHubModelsOptions
{
    public const string SectionName = "AiProviders:GitHubModels";

    public string Endpoint { get; init; } = "https://models.github.ai/inference/chat/completions";

    public string ApiVersion { get; init; } = "2026-03-10";

    public string Model { get; init; } = "openai/gpt-4.1-mini";

    public int MaxTokens { get; init; } = 2_000;

    public decimal Temperature { get; init; } = 0.2m;

    public string Token { get; init; } = string.Empty;
}
