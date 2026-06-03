using AiTravelPlanner.Application.Trips.Prompting;
using AiTravelPlanner.Application.Trips.Sanitization;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Application.Trips.UseCases.GetTrip;
using AiTravelPlanner.Application.Trips.UseCases.ListTrips;
using AiTravelPlanner.Application.Trips.UseCases.ValidateTrip;
using AiTravelPlanner.Application.Trips.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace AiTravelPlanner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITripPlanValidator, TripPlanValidator>();
        services.AddScoped<IGenerateTripUseCase, GenerateTripHandler>();
        services.AddScoped<IValidateTripUseCase, ValidateTripHandler>();
        services.AddScoped<IGetTripUseCase, GetTripHandler>();
        services.AddScoped<IListTripsUseCase, ListTripsHandler>();

        services.AddSingleton<ITripInputSanitizer, TripInputSanitizer>();
        services.AddSingleton<ITripGenerationPromptBuilder, TripGenerationPromptBuilder>();

        return services;
    }
}
