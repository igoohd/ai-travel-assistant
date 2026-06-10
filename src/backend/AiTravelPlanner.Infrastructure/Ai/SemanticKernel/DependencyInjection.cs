using AiTravelPlanner.Application.Trips.Planning;
using AiTravelPlanner.Infrastructure.Ai.SemanticKernel.Filters;
using AiTravelPlanner.Infrastructure.Ai.SemanticKernel.Plugins;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace AiTravelPlanner.Infrastructure.Ai.SemanticKernel;

internal static class DependencyInjection
{
    public static IServiceCollection AddSemanticKernel(
        this IServiceCollection services)
    {
        services.AddSingleton<Kernel>(serviceProvider =>
        {
            var chatClient = serviceProvider.GetRequiredService<IChatClient>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            var semanticKernelChatClient = chatClient
                .AsBuilder()
                .UseKernelFunctionInvocation(loggerFactory)
                .Build(serviceProvider);

            var kernelBuilder = Kernel.CreateBuilder();
            kernelBuilder.Services.AddSingleton(semanticKernelChatClient);

            var dailyBudgetCalculator =
                serviceProvider.GetRequiredService<DailyBudgetCalculator>();

            kernelBuilder.Plugins.AddFromObject(
                new TripPlanningPlugin(dailyBudgetCalculator),
                "TripPlanning");

            var kernel = kernelBuilder.Build();

            kernel.AutoFunctionInvocationFilters.Add(
                new AutoFunctionLoggingFilter(
                    serviceProvider.GetRequiredService<
                        ILogger<AutoFunctionLoggingFilter>>()));

            return kernel;
        });

        return services;
    }
}
