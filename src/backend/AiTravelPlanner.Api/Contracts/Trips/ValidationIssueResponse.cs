namespace AiTravelPlanner.Api.Contracts.Trips;

public sealed record ValidationIssueResponse(
    string Code,
    string Message,
    string Severity);
