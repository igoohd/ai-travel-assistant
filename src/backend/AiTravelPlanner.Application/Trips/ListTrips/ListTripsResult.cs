using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.ListTrips;

public sealed record ListTripsResult(IReadOnlyList<Plan> Plans);
