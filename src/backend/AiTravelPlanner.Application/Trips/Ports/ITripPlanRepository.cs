using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.Ports;

public interface ITripPlanRepository
{
    Task SaveAsync(
        Plan plan,
        GenerateTripCommand command,
        IReadOnlyList<ValidationIssue> validationIssues,
        CancellationToken cancellationToken);

    Task<StoredTrip?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<StoredTrip>> ListAsync(CancellationToken cancellationToken);
}
