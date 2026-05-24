namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public sealed record GenerateTripCommand(
    string Destination,
    int NumberOfDays,
    decimal Budget,
    string Currency,
    IReadOnlyCollection<string> Interests);