using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Application.Trips.Services;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Infrastructure.Persistence;

public sealed class InMemoryTripPlanRepository : ITripPlanRepository
{
    private static readonly Dictionary<Guid, Plan> Plans = new();

    public Task SaveAsync(
        Plan plan,
        GenerateTripCommand command,
        IReadOnlyList<ValidationIssue> validationIssues,
        CancellationToken cancellationToken)
    {
        Plans[plan.Id] = plan;
        return Task.CompletedTask;
    }

    public Task<Plan?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Plans.TryGetValue(id, out var plan);
        return Task.FromResult(plan);
    }

    public Task<IReadOnlyList<Plan>> ListAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<Plan> plans = Plans.Values
            .OrderByDescending(p => p.CreatedAt)
            .ToArray();

        return Task.FromResult(plans);
    }
}