using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public interface IGenerateTripUseCase
{
    Task<GenerateTripResult> HandleAsync(GenerateTripCommand command, CancellationToken cancellationToken);
}
