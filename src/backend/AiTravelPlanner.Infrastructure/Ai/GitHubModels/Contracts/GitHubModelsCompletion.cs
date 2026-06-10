namespace AiTravelPlanner.Infrastructure.Ai.GitHubModels.Contracts;

public sealed record GitHubModelsCompletion(
    string Content,
    GitHubModelsUsage? Usage);
