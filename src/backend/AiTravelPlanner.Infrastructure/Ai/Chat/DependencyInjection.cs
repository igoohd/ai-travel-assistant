using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;

namespace AiTravelPlanner.Infrastructure.Ai.Chat;

internal static class DependencyInjection
{
    public static IServiceCollection AddAiChatClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<GitHubModelsChatOptions>()
            .Bind(configuration.GetSection(GitHubModelsChatOptions.SectionName))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Endpoint),
                "Chat client endpoint is required.")
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Model),
                "Chat client model is required.")
            .Validate(
                options => options.MaxTokens > 0,
                "Chat client max tokens must be greater than zero.")
            .Validate(
                options => options.Temperature is >= 0 and <= 1,
                "Chat client temperature must be between 0 and 1.")
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Token),
                "Chat client token is required.");

        services.AddSingleton<IChatClient>(serviceProvider =>
        {
            var options = serviceProvider
                .GetRequiredService<IOptions<GitHubModelsChatOptions>>()
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
}
