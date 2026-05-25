namespace AiTravelPlanner.Infrastructure.Ai.GitHubModels;

public interface IGitHubModelsClient
{
    Task<string> CompleteChatAsync(
        IReadOnlyList<GitHubModelsMessage> messages,
        CancellationToken cancellationToken = default);
}