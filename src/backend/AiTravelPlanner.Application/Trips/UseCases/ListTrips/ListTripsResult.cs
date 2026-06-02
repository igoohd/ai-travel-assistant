using AiTravelPlanner.Application.Trips.Ports;

namespace AiTravelPlanner.Application.Trips.UseCases.ListTrips;

public sealed record ListTripsResult(IReadOnlyList<StoredTrip> Trips);

