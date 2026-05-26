using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.Services;

public sealed class TripPlanValidator : ITripPlanValidator
{
    public IReadOnlyList<ValidationIssue> Validate(Plan plan, GenerateTripCommand command)
    {
        var issues = new List<ValidationIssue>();

        if (plan.Days.Any(day => day.Activities.Count > 4))
        {
            issues.Add(new ValidationIssue(
                Code: ValidationIssueCodes.TooManyActivities,
                Message: "One or more days has too many activities.",
                Severity: ValidationSeverity.Warning));
        }

        var estimatedExperienceCost = plan.Days.Sum(day =>
            day.Activities.Sum(activity => activity.EstimatedCost)
            + day.Restaurants.Sum(restaurant => restaurant.EstimatedCost));

        var plannedExperienceBudget = plan.Budget.Food + plan.Budget.Activities;

        if (estimatedExperienceCost > plannedExperienceBudget)
        {
            issues.Add(new ValidationIssue(
                Code: ValidationIssueCodes.BudgetExceeded,
                Message: "Estimated activity and restaurant costs exceed the planned experience budget.",
                Severity: ValidationSeverity.Warning));
        }

        if (plan.Budget.Total > command.Budget)
        {
            issues.Add(new ValidationIssue(
                Code: ValidationIssueCodes.BudgetExceeded,
                Message: "Estimated trip cost exceeds the requested budget.",
                Severity: ValidationSeverity.Warning));
        }

        if (plan.Days.Any(day => day.Activities.Sum(activity => activity.DurationHours) > 10))
        {
            issues.Add(new ValidationIssue(
                Code: ValidationIssueCodes.UnrealisticSchedule,
                Message: "One or more days has more than 10 hours of planned activities.",
                Severity: ValidationSeverity.Warning));
        }

        if (plan.Days.Any(day => day.Activities.Sum(activity => activity.TransitMinutesFromPrevious) > 180))
        {
            issues.Add(new ValidationIssue(
                Code: ValidationIssueCodes.UnrealisticTransportation,
                Message: "One or more days has more than 3 hours of estimated transit time.",
                Severity: ValidationSeverity.Warning));
        }

        return issues;
    }
}
