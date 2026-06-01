using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public sealed record GenerateTripResult(
    Plan? Plan,
    IReadOnlyList<ValidationIssue> ValidationIssues,
    int RetryCount,
    int DurationMs,
    IReadOnlyList<string> RetryReasons,
    IReadOnlyList<string> Errors)
{
    public bool IsSuccess => Errors.Count == 0;

    public static GenerateTripResult Success(
        Plan plan,
        IReadOnlyList<ValidationIssue> validationIssues,
        int retryCount,
        IReadOnlyList<string> retryReasons,
        int durationMs)
    {
        return new GenerateTripResult(plan, validationIssues, retryCount, durationMs, retryReasons, []);
    }

    public static GenerateTripResult Failure(IReadOnlyList<string> errors)
    {
        return new GenerateTripResult(null, [], 0, 0, [], errors);
    }
}
