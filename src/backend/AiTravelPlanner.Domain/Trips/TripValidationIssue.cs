public sealed record TripValidationIssue(
    string Code,
    string Message,
    string Severity
);