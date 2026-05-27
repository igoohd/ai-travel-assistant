using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public sealed record GenerateTripResult(
    Plan? Plan,
    IReadOnlyList<ValidationIssue> ValidationIssues,
    int RetryCount,
    IReadOnlyList<string> Errors)
{
    public bool IsSuccess => Errors.Count == 0;

    public static GenerateTripResult Success(
        Plan plan,
        IReadOnlyList<ValidationIssue> validationIssues,
        int retryCount)
    {
        return new GenerateTripResult(plan, validationIssues, retryCount, []);
    }

    public static GenerateTripResult Failure(IReadOnlyList<string> errors)
    {
        return new GenerateTripResult(null, [], 0, errors);
    }
}
