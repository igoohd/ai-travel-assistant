namespace AiTravelPlanner.Api.Contracts.Trips;

public sealed record GenerateTripResponse(
    string Destination,
    int NumberOfDays,
    string Overview);