using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Api.Contracts.Trips.TripDetails;

internal static class TripDetailsMapper
{
    public static TripDetailsResponse ToResponse(
        Plan plan,
        IReadOnlyList<ValidationIssue> validationIssues,
        int retryCount,
        IReadOnlyList<string> retryReasons,
        int durationMs)
    {
        return new TripDetailsResponse(
            Id: plan.Id,
            Destination: plan.Destination,
            NumberOfDays: plan.NumberOfDays,
            Days: plan.Days
                .Select(day => new DayResponse(
                    DayNumber: day.DayNumber,
                    Title: day.Title,
                    Description: day.Description,
                    Activities: day.Activities
                        .Select(activity => new ActivityResponse(
                            TimeOfDay: activity.TimeOfDay,
                            Title: activity.Title,
                            Description: activity.Description,
                            EstimatedCost: activity.EstimatedCost,
                            DurationHours: activity.DurationHours,
                            TransitMinutesFromPrevious:
                                activity.TransitMinutesFromPrevious))
                        .ToArray(),
                    Restaurants: day.Restaurants
                        .Select(restaurant => new RestaurantSuggestionResponse(
                            Name: restaurant.Name,
                            Cuisine: restaurant.Cuisine,
                            Notes: restaurant.Notes,
                            EstimatedCost: restaurant.EstimatedCost))
                        .ToArray()))
                .ToArray(),
            Budget: new BudgetEstimateResponse(
                Hotel: plan.Budget.Hotel,
                Transportation: plan.Budget.Transportation,
                Food: plan.Budget.Food,
                Activities: plan.Budget.Activities,
                Total: plan.Budget.Total,
                Category: plan.Budget.Category,
                Currency: plan.Budget.Currency.Value),
            Summary: new SummaryResponse(
                Overview: plan.Summary.Overview,
                Highlights: plan.Summary.Highlights,
                TravelTips: plan.Summary.TravelTips),
            ValidationIssues: validationIssues
                .Select(issue => new ValidationIssueResponse(
                    Code: issue.Code,
                    Message: issue.Message,
                    Severity: issue.Severity.ToString()))
                .ToArray(),
            AiMetadata: new AiGenerationResponse(
                Provider: plan.AiMetadata.Provider,
                Model: plan.AiMetadata.Model,
                PromptTokens: plan.AiMetadata.PromptTokens,
                CompletionTokens: plan.AiMetadata.CompletionTokens,
                TotalTokens: plan.AiMetadata.TotalTokens),
            Diagnostics: new GenerationDiagnosticsResponse(
                RetryCount: retryCount,
                RetryReasons: retryReasons,
                DurationMs: durationMs));
    }
}
