using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Infrastructure.Ai.AgentFramework;
using AiTravelPlanner.Infrastructure.Ai.Chat;
using AiTravelPlanner.Infrastructure.Ai.ExtensionsAi;
using AiTravelPlanner.Infrastructure.Ai.GitHubModels;
using AiTravelPlanner.Infrastructure.Ai.SemanticKernel;
using AiTravelPlanner.Infrastructure.Ai.Stub;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AiTravelPlanner.Infrastructure.Ai;

internal static class DependencyInjection
{
    public static IServiceCollection AddAi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddGitHubModels(configuration);
        services.AddAiChatClient(configuration);
        services.AddSemanticKernel();
        services.AddAgentFramework();
        services.AddTripPlanGenerator(configuration);

        return services;
    }

    private static void AddTripPlanGenerator(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration
            .GetSection(AiProviderOptions.SectionName)
            .Get<AiProviderOptions>() ?? new AiProviderOptions();

        if (options.ActiveProvider.Equals(
            AiProviderNames.GitHubModels,
            StringComparison.OrdinalIgnoreCase))
        {
            services.AddScoped<ITripPlanGenerator, GitHubModelsTripPlanGenerator>();
        }
        else if (options.ActiveProvider.Equals(
            AiProviderNames.ExtensionsAi,
            StringComparison.OrdinalIgnoreCase))
        {
            services.AddScoped<ITripPlanGenerator, ExtensionsAiTripPlanGenerator>();
        }
        else if (options.ActiveProvider.Equals(
            AiProviderNames.SemanticKernel,
            StringComparison.OrdinalIgnoreCase))
        {
            services.AddScoped<ITripPlanGenerator, SemanticKernelTripPlanGenerator>();
        }
        else if (options.ActiveProvider.Equals(
            AiProviderNames.AgentFramework,
            StringComparison.OrdinalIgnoreCase))
        {
            services.AddScoped<ITripPlanGenerator, AgentFrameworkTripPlanGenerator>();
        }
        else if (options.ActiveProvider.Equals(
            AiProviderNames.Stub,
            StringComparison.OrdinalIgnoreCase))
        {
            services.AddScoped<ITripPlanGenerator, StubTripPlanGenerator>();
        }
        else
        {
            throw new InvalidOperationException(
                $"Unsupported AI provider '{options.ActiveProvider}'.");
        }
    }
}
