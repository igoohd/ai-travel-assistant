namespace AiTravelPlanner.Api.Contracts.Trips.TripDetails;

public sealed record ActivityResponse(
    string TimeOfDay,
    string Title,
    string Description,
    decimal EstimatedCost,
    decimal DurationHours,
    decimal TransitMinutesFromPrevious);
