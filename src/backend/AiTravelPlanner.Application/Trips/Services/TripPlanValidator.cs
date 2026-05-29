using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.Services;

public sealed class TripPlanValidator : ITripPlanValidator
{
    public IReadOnlyList<ValidationIssue> Validate(Plan plan)
    {
        var issues = new List<ValidationIssue>();

        if (plan.Days.Any(day => day.Activities.Count > 4))
        {
            issues.Add(new ValidationIssue(
                ValidationIssueCodes.TooManyActivities,
                "One or more days has too many activities.",
                ValidationSeverity.Warning));
        }

        if (plan.Days.Any(day => day.Activities.Sum(activity => activity.DurationHours) > 10))
        {
            issues.Add(new ValidationIssue(
                ValidationIssueCodes.UnrealisticSchedule,
                "One or more days has more than 10 hours of planned activities.",
                ValidationSeverity.Warning));
        }

        if (plan.Days.Any(day => day.Activities.Sum(activity => activity.TransitMinutesFromPrevious) > 180))
        {
            issues.Add(new ValidationIssue(
                ValidationIssueCodes.UnrealisticTransportation,
                "One or more days has more than 3 hours of estimated transit time.",
                ValidationSeverity.Warning));
        }

        return issues;
    }
    public IReadOnlyList<ValidationIssue> Validate(Plan plan, GenerateTripCommand command)
    {
        var issues = Validate(plan).ToList();

        if (plan.Budget.Total > command.Budget)
        {
            issues.Add(new ValidationIssue(
                ValidationIssueCodes.BudgetExceeded,
                $"The total estimated cost of the trip (${plan.Budget.Total:F2}) exceeds the specified budget (${command.Budget:F2}).",
                ValidationSeverity.Warning));
        }

        return issues;
    }
}
