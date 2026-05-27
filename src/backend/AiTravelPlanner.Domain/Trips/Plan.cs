namespace AiTravelPlanner.Domain.Trips;

public sealed record Plan(
    Guid Id,
    string Destination,
    int NumberOfDays,
    IReadOnlyList<Day> Days,
    BudgetEstimate Budget,
    Summary Summary,
    DateTimeOffset CreatedAt
);
