using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.Services;

public interface ITripPlanGenerator
{
    Task<Plan> GenerateAsync(GenerateTripCommand command);
}
