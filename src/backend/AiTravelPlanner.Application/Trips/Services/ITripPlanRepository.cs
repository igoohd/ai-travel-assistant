using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.Services;

public interface ITripPlanRepository
{
    Task SaveAsync(
        Plan plan,
        GenerateTripCommand command,
        IReadOnlyList<ValidationIssue> validationIssues,
        CancellationToken cancellationToken);
}