using System.Text.Json.Serialization;

namespace AiTravelPlanner.Infrastructure.Ai.GitHubModels;

public sealed record GitHubModelsChatCompletionResponse(
    [property: JsonPropertyName("choices")]
    IReadOnlyList<GitHubModelsChoice> Choices);

public sealed record GitHubModelsChoice(
    [property: JsonPropertyName("message")]
    GitHubModelsResponseMessage Message);

public sealed record GitHubModelsResponseMessage(
    [property: JsonPropertyName("content")]
string Content);