using AiTravelPlanner.Infrastructure.Ai;
using AiTravelPlanner.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AiTravelPlanner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAi(configuration);
        services.AddPersistence();

        return services;
    }
}
