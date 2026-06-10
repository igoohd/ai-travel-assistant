namespace AiTravelPlanner.Api.Contracts.Trips.TripDetails;

public sealed record BudgetEstimateResponse(
    decimal Hotel,
    decimal Transportation,
    decimal Food,
    decimal Activities,
    decimal Total,
    string Category,
    string Currency);
