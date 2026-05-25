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

        return issues;
    }
}
