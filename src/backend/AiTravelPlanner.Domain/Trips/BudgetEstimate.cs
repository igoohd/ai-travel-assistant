public sealed record BudgetEstimate(
    decimal Hotel,
    decimal Transportation,
    decimal Food,
    decimal Activities,
    decimal Total,
    string Category);