namespace AiTravelPlanner.Api.Contracts.Trips.ListTrips;

public sealed record TripListItemResponse(
    Guid Id,
    DateTimeOffset CreatedAt,
    string Destination,
    int NumberOfDays,
    decimal EstimatedTotal,
    string Currency,
    string BudgetCategory,
    int ValidationIssueCount,
    AiGenerationResponse AiMetadata
    );