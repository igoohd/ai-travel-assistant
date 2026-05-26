using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Application.Trips.Services;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Infrastructure.Persistence;

public sealed class NoOpTripPlanRepository : ITripPlanRepository
{
    public Task SaveAsync(
        Plan plan,
        GenerateTripCommand command,
        IReadOnlyList<ValidationIssue> validationIssues,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}