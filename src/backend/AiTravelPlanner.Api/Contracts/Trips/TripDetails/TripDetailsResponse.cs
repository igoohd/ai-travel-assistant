namespace AiTravelPlanner.Api.Contracts.Trips.TripDetails;

public sealed record TripDetailsResponse(
    Guid Id,
    string Destination,
    int NumberOfDays,
    IReadOnlyCollection<DayResponse> Days,
    BudgetEstimateResponse Budget,
    SummaryResponse Summary,
    IReadOnlyCollection<ValidationIssueResponse> ValidationIssues,
    AiGenerationResponse AiMetadata,
    GenerationDiagnosticsResponse Diagnostics);
