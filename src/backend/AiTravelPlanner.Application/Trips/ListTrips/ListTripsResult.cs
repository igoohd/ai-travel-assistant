using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.ListTrips;

public sealed record ListTripsResult(IReadOnlyList<Plan> Plans)
{
        public bool IsFound => Plans is not null && Plans.Count > 0;
};