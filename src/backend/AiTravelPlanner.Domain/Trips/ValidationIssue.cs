namespace AiTravelPlanner.Domain.Trips;

public sealed record ValidationIssue(
    string Code,
    string Message,
    string Severity
);
