using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Infrastructure.Ai.ExtensionsAi;

public sealed class ExtensionsAiTripPlanGenerator : ITripPlanGenerator
{

    public Task<Plan> GenerateAsync(GenerateTripCommand command, CancellationToken cancellationToken, string? additionalInstruction = null)
    {
        throw new NotImplementedException("Extensions AI integration is not implemented yet.");
    }
}