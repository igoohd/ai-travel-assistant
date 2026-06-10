using AiTravelPlanner.Application.Trips.Planning;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AiTravelPlanner.Infrastructure.Ai.AgentFramework;

internal static class DependencyInjection
{
    public static IServiceCollection AddAgentFramework(
        this IServiceCollection services)
    {
        services.AddPlannerAgent();
        services.AddValidatorAgent();
        services.AddScoped<AgentFrameworkTripPlanReviewer>();

        return services;
    }

    private static void AddPlannerAgent(this IServiceCollection services)
    {
        services.AddKeyedSingleton<ChatClientAgent>(
            AgentKeys.Planner,
            (serviceProvider, _) =>
            {
                var chatClient = serviceProvider.GetRequiredService<IChatClient>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("AgentFrameworkTools");
                var dailyBudgetCalculator =
                    serviceProvider.GetRequiredService<DailyBudgetCalculator>();

                var calculateDailyBudget = AIFunctionFactory.Create(
                    (decimal totalBudget, int numberOfDays) =>
                    {
                        logger.LogInformation(
                            "Daily budget tool called: {Budget} / {Days}",
                            totalBudget,
                            numberOfDays);

                        return dailyBudgetCalculator.Calculate(
                            totalBudget,
                            numberOfDays);
                    },
                    name: "calculate_daily_budget",
                    description: "Calculates the available daily budget.");

                return chatClient.AsAIAgent(
                    name: "TravelPlanner",
                    description: "Creates structured travel itineraries.",
                    instructions: AgentInstructions.Planner,
                    tools: [calculateDailyBudget],
                    loggerFactory: loggerFactory,
                    services: serviceProvider);
            });
    }

    private static void AddValidatorAgent(this IServiceCollection services)
    {
        services.AddKeyedSingleton<ChatClientAgent>(
            AgentKeys.Validator,
            (serviceProvider, _) =>
            {
                var chatClient = serviceProvider.GetRequiredService<IChatClient>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                return chatClient.AsAIAgent(
                    name: "TripValidator",
                    description: "Reviews travel itineraries for practical problems.",
                    instructions: AgentInstructions.Validator,
                    loggerFactory: loggerFactory,
                    services: serviceProvider);
            });
    }
}
