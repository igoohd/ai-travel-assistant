using AiTravelPlanner.Infrastructure.Ai.GitHubModels.Contracts;

namespace AiTravelPlanner.Infrastructure.Ai.GitHubModels;

public interface IGitHubModelsClient
{
    Task<GitHubModelsCompletion> CompleteChatAsync(
        IReadOnlyList<GitHubModelsMessage> messages,
        CancellationToken cancellationToken);
}
