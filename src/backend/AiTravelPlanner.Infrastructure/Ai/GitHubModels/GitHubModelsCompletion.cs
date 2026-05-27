using AiTravelPlanner.Infrastructure.Ai.GitHubModels;

public sealed record GitHubModelsCompletion(
    string Content,
    GitHubModelsUsage? Usage);