namespace AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;

public sealed record GenerateTripResponse(
    string Destination,
    int NumberOfDays,
    string Overview,
    IReadOnlyCollection<DayResponse> Days,
    BudgetEstimateResponse Budget,
    IReadOnlyCollection<string> Highlights,
    IReadOnlyCollection<string> TravelTips,
    IReadOnlyCollection<ValidationIssueResponse> ValidationIssues);
