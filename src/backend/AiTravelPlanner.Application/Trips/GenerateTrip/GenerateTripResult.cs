using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public sealed record GenerateTripResult(
    Plan? Plan,
    IReadOnlyList<ValidationIssue> ValidationIssues,
    IReadOnlyList<string> Errors)
{
    public bool IsSuccess => Errors.Count == 0;

    public static GenerateTripResult Success(
        Plan plan,
        IReadOnlyList<ValidationIssue> validationIssues)
    {
        return new GenerateTripResult(plan, validationIssues, []);
    }

    public static GenerateTripResult Failure(IReadOnlyList<string> errors)
    {
        return new GenerateTripResult(null, [], errors);
    }
}
