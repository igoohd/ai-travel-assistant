namespace AiTravelPlanner.Domain.Trips;

public sealed record Plan(
    string Destination,
    int NumberOfDays,
    IReadOnlyList<Day> Days,
    BudgetEstimate Budget,
    Summary Summary
);
