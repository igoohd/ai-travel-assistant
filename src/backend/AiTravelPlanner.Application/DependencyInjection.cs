using AiTravelPlanner.Application.Trips;
using Microsoft.Extensions.DependencyInjection;

namespace AiTravelPlanner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddTrips();
    }
}
