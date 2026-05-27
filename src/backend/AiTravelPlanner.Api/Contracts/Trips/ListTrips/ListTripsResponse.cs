namespace AiTravelPlanner.Api.Contracts.Trips.ListTrips;

public sealed record ListTripsResponse(IReadOnlyList<TripListItemResponse> Trips);