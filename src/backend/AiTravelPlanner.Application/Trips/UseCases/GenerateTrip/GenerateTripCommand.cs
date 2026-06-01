namespace AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;

public sealed record GenerateTripCommand(
    string Destination,
    int NumberOfDays,
    decimal Budget,
    string Currency,
    IReadOnlyCollection<string> Interests);
