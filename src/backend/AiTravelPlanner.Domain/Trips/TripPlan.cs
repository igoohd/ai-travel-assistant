namespace AiTravelPlanner.Domain.Trips;

public sealed record TripPlan(
    string Destination,
    int NumberOfDays,
    string Overview,
    IReadOnlyList<TripDay> Days);