namespace AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;

public sealed record GenerateTripResponse(
    Guid Id,
    string Destination,
    int NumberOfDays,
    IReadOnlyCollection<DayResponse> Days,
    BudgetEstimateResponse Budget,
    SummaryResponse Summary,
    IReadOnlyCollection<ValidationIssueResponse> ValidationIssues,
    AiGenerationResponse AiMetadata,
    GenerationDiagnosticsResponse Diagnostics);
