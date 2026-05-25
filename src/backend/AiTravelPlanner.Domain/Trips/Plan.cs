namespace AiTravelPlanner.Domain.Trips;

public sealed record Plan(
    string Destination,
    int NumberOfDays,
    string Overview,
    IReadOnlyList<Day> Days,
    BudgetEstimate Budget,
    IReadOnlyList<string> Highlights,
    IReadOnlyList<string> TravelTips
);
