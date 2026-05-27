using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.ValidateTrip;

public sealed record ValidateTripResult(IReadOnlyList<ValidationIssue> ValidationIssues, IReadOnlyList<string> Errors)
{
    public bool IsSuccessful => Errors.Count == 0;

    public static ValidateTripResult Success(IReadOnlyList<ValidationIssue> issues)
    {
        return new ValidateTripResult(issues, Array.Empty<string>());
    }

    public static ValidateTripResult Failure(IReadOnlyList<string> errors)
    {
        return new ValidateTripResult(Array.Empty<ValidationIssue>(), errors);
    }
}