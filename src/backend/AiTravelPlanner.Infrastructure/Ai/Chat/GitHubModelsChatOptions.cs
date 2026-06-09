namespace AiTravelPlanner.Infrastructure.Ai.Chat;

public sealed class GitHubModelsChatOptions
{
    public const string SectionName = "AiProviders:ChatClient";

    public string Endpoint { get; init; } = "https://models.github.ai/inference/";

    public string Model { get; init; } = "openai/gpt-4.1-mini";

    public int MaxTokens { get; init; } = 2_000;

    public decimal Temperature { get; init; } = 0.2m;

    public string Token { get; init; } = string.Empty;
}
