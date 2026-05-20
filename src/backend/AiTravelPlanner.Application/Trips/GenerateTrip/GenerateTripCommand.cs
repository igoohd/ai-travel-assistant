namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public sealed record GenerateTripCommand(
    string Destination,
    int NumberOfDays,
    decimal Budget,
    IReadOnlyCollection<string> Interests);