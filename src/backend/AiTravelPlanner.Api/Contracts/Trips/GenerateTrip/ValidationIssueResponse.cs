namespace AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;

public sealed record ValidationIssueResponse(
    string Code,
    string Message,
    string Severity);
