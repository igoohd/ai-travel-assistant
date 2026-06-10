using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AiTravelPlanner.Infrastructure.Ai.GitHubModels;

internal static class DependencyInjection
{
    public static IServiceCollection AddGitHubModels(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<GitHubModelsOptions>()
            .Bind(configuration.GetSection(GitHubModelsOptions.SectionName))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Endpoint),
                "GitHub Models endpoint is required.")
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Model),
                "GitHub Models model is required.")
            .Validate(
                options => options.MaxTokens > 0,
                "GitHub Models max tokens must be greater than zero.")
            .Validate(
                options => options.Temperature is >= 0 and <= 1,
                "GitHub Models temperature must be between 0 and 1.");

        services.AddHttpClient<IGitHubModelsClient, GitHubModelsClient>();

        return services;
    }
}
