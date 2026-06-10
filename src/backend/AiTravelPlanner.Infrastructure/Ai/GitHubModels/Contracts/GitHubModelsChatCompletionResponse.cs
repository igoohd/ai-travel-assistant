using System.Text.Json.Serialization;

namespace AiTravelPlanner.Infrastructure.Ai.GitHubModels.Contracts;

public sealed record GitHubModelsChatCompletionResponse(
    [property: JsonPropertyName("choices")]
    IReadOnlyList<GitHubModelsChoice> Choices,

    [property: JsonPropertyName("usage")]
    GitHubModelsUsage? Usage);

public sealed record GitHubModelsChoice(
    [property: JsonPropertyName("message")]
    GitHubModelsResponseMessage Message);

public sealed record GitHubModelsResponseMessage(
    [property: JsonPropertyName("content")]
    string Content);

public sealed record GitHubModelsUsage(
    [property: JsonPropertyName("prompt_tokens")]
    int PromptTokens,

    [property: JsonPropertyName("completion_tokens")]
    int CompletionTokens,

    [property: JsonPropertyName("total_tokens")]
    int TotalTokens);
