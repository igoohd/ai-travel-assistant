namespace AiTravelPlanner.Domain.Trips;

public sealed record BudgetEstimate(
    decimal Hotel,
    decimal Transportation,
    decimal Food,
    decimal Activities,
    decimal Total,
    CurrencyCode Currency,
    string Category);
