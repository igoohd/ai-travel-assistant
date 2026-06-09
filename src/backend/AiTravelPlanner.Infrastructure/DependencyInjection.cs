using System.ClientModel;
using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Infrastructure.Ai;
using AiTravelPlanner.Infrastructure.Ai.ExtensionsAi;
using AiTravelPlanner.Infrastructure.Ai.GitHubModels;
using AiTravelPlanner.Infrastructure.Ai.SemanticKernel;
using AiTravelPlanner.Infrastructure.Ai.SemanticKernel.Plugins;
using AiTravelPlanner.Infrastructure.Ai.Stub;
using AiTravelPlanner.Infrastructure.Persistence;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using OpenAI;
using OpenAI.Chat;

namespace AiTravelPlanner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTripPlanGenerator(configuration);
        services.AddGitHubModels(configuration);
        services.AddExtensionsAi(configuration);
        services.AddSemanticKernel();
        services.AddSingleton<ITripPlanRepository, InMemoryTripPlanRepository>();

        return services;
    }

    private static IServiceCollection AddTripPlanGenerator(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var aiProviderOptions = configuration
            .GetSection(AiProviderOptions.SectionName)
            .Get<AiProviderOptions>() ?? new AiProviderOptions();

        if (aiProviderOptions.ActiveProvider.Equals("GitHubModels", StringComparison.OrdinalIgnoreCase))
        {
            services.AddScoped<ITripPlanGenerator, GitHubModelsTripPlanGenerator>();
        }
        else if (aiProviderOptions.ActiveProvider.Equals("ExtensionsAi", StringComparison.OrdinalIgnoreCase))
        {
            services.AddScoped<ITripPlanGenerator, ExtensionsAiTripPlanGenerator>();
        }
        else if (aiProviderOptions.ActiveProvider.Equals("SemanticKernel", StringComparison.OrdinalIgnoreCase))
        {
            services.AddScoped<ITripPlanGenerator, SemanticKernelTripPlanGenerator>();
        }
        else
        {
            services.AddScoped<ITripPlanGenerator, StubTripPlanGenerator>();
        }

        return services;
    }

    private static IServiceCollection AddGitHubModels(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<GitHubModelsOptions>()
            .Bind(configuration.GetSection(GitHubModelsOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.Endpoint), "GitHub Models endpoint is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Model), "GitHub Models model is required.")
            .Validate(options => options.MaxTokens > 0, "GitHub Models max tokens must be greater than zero.")
            .Validate(options => options.Temperature >= 0 && options.Temperature <= 1, "GitHub Models temperature must be between 0 and 1.");

        services.AddHttpClient<IGitHubModelsClient, GitHubModelsClient>();

        return services;
    }

    private static IServiceCollection AddExtensionsAi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<ExtensionsAiOptions>()
            .Bind(configuration.GetSection(ExtensionsAiOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.Endpoint), "Extensions AI endpoint is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Model), "Extensions AI model is required.")
            .Validate(options => options.MaxTokens > 0, "Extensions AI max tokens must be greater than zero.")
            .Validate(options => options.Temperature >= 0 && options.Temperature <= 1, "Extensions AI temperature must be between 0 and 1.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Token), "Extensions AI token is required.");

        services.AddSingleton<IChatClient>(serviceProvider =>
        {
            var options = serviceProvider
                .GetRequiredService<IOptions<ExtensionsAiOptions>>()
                .Value;

            var chatClient = new ChatClient(
                model: options.Model,
                credential: new ApiKeyCredential(options.Token),
                options: new OpenAIClientOptions
                {
                    Endpoint = new Uri(options.Endpoint)
                });

            return chatClient.AsIChatClient();
        });

        return services;
    }

    private static IServiceCollection AddSemanticKernel(this IServiceCollection services)
    {
        services.AddSingleton<Kernel>(serviceProvider =>
        {
            var chatClient = serviceProvider.GetRequiredService<IChatClient>();
            var kernelBuilder = Kernel.CreateBuilder();

            kernelBuilder.Services.AddSingleton(chatClient);

            kernelBuilder.Plugins.AddFromType<TripPlanningPlugin>("TripPlanning");

            return kernelBuilder.Build();
        });

        return services;
    }
}
