using AiTravelPlanner.Application.Trips.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace AiTravelPlanner.Infrastructure.Persistence;

internal static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services)
    {
        services.AddSingleton<ITripPlanRepository, InMemoryTripPlanRepository>();

        return services;
    }
}
