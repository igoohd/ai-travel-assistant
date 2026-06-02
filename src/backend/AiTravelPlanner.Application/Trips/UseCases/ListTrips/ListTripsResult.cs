using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.UseCases.ListTrips;

public sealed record ListTripsResult(IReadOnlyList<StoredTrip> Trips);

