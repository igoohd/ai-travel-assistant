using System.Collections.Concurrent;
using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Infrastructure.Persistence;

public sealed class InMemoryTripPlanRepository : ITripPlanRepository
{
    private static readonly ConcurrentDictionary<Guid, StoredTrip> Trips = new();

    public Task SaveAsync(
        Plan plan,
        GenerateTripCommand command,
        int retryCount,
        IReadOnlyList<string> retryReasons,
        int durationMs,
        IReadOnlyList<ValidationIssue> validationIssues,
        CancellationToken cancellationToken)
    {
        Trips[plan.Id] = new StoredTrip(plan, command, retryCount, retryReasons, durationMs, validationIssues);
        return Task.CompletedTask;
    }

    public Task<StoredTrip?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Trips.TryGetValue(id, out var storedTrip);
        return Task.FromResult(storedTrip);
    }

    public Task<IReadOnlyList<StoredTrip>> ListAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<StoredTrip> trips = Trips.Values
            .OrderByDescending(st => st.Plan.CreatedAt)
            .ToArray();

        return Task.FromResult(trips);
    }
}
