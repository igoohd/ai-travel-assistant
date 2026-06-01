using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.Ports;

public interface ITripPlanGenerator
{
    Task<Plan> GenerateAsync(
        GenerateTripCommand command,
        CancellationToken cancellationToken,
        string? additionalInstruction = null);
}
