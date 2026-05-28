namespace AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;

public sealed record ActivityResponse(
    string TimeOfDay,
    string Title,
    string Description,
    decimal EstimatedCost,
    decimal DurationHours,
    decimal TransitMinutesFromPrevious);
