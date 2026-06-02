using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Application.Trips.UseCases.GetTrip;

namespace AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;

public static class GenerateTripMapper
{
    public static GenerateTripCommand ToCommand(this GenerateTripRequest request)
    {
        return new GenerateTripCommand(
            Destination: request.Destination,
            NumberOfDays: request.NumberOfDays,
            Budget: request.Budget,
            Currency: request.Currency,
            Interests: request.Interests
                .Where(interest => !string.IsNullOrWhiteSpace(interest))
                .Select(interest => interest.Trim())
                .ToArray());
    }

    public static GenerateTripResponse ToResponse(this GenerateTripResult result)
    {
        var tripPlan = result.Plan
            ?? throw new InvalidOperationException("Cannot map a failed trip generation result to a response.");

        var days = tripPlan.Days
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
                        TransitMinutesFromPrevious: activity.TransitMinutesFromPrevious))
                    .ToArray(),
                Restaurants: day.Restaurants
                    .Select(restaurant => new RestaurantSuggestionResponse(
                        Name: restaurant.Name,
                        Cuisine: restaurant.Cuisine,
                        Notes: restaurant.Notes,
                        EstimatedCost: restaurant.EstimatedCost))
                    .ToArray())
            )
            .ToArray();

        return new GenerateTripResponse(
            Id: tripPlan.Id,
            Destination: tripPlan.Destination,
            NumberOfDays: tripPlan.NumberOfDays,
            Days: days,
            Budget: new BudgetEstimateResponse(
                Hotel: tripPlan.Budget.Hotel,
                Transportation: tripPlan.Budget.Transportation,
                Food: tripPlan.Budget.Food,
                Activities: tripPlan.Budget.Activities,
                Total: tripPlan.Budget.Total,
                Category: tripPlan.Budget.Category,
                Currency: tripPlan.Budget.Currency.Value),
            Summary: new SummaryResponse(
                Overview: tripPlan.Summary.Overview,
                Highlights: tripPlan.Summary.Highlights,
                TravelTips: tripPlan.Summary.TravelTips),
            ValidationIssues: result.ValidationIssues
                .Select(issue => new ValidationIssueResponse(
                    Code: issue.Code,
                    Message: issue.Message,
                    Severity: issue.Severity.ToString()))
                .ToArray(),
            AiMetadata: new AiGenerationResponse(
                Provider: tripPlan.AiMetadata.Provider,
                Model: tripPlan.AiMetadata.Model,
                PromptTokens: tripPlan.AiMetadata.PromptTokens,
                CompletionTokens: tripPlan.AiMetadata.CompletionTokens,
                TotalTokens: tripPlan.AiMetadata.TotalTokens),
            Diagnostics: new GenerationDiagnosticsResponse(
                RetryCount: result.RetryCount,
                RetryReasons: result.RetryReasons,
                DurationMs: result.DurationMs)
        );
    }

    public static GenerateTripResponse ToResponse(this GetTripResult result)
    {
        var storedTrip = result.Trip
            ?? throw new InvalidOperationException("Cannot map a missing trip to a response.");

        return new GenerateTripResult(
            Plan: storedTrip.Plan,
            ValidationIssues: storedTrip.ValidationIssues,
            RetryCount: storedTrip.RetryCount,
            RetryReasons: storedTrip.RetryReasons,
            DurationMs: storedTrip.DurationMs,
            Errors: [])
            .ToResponse();
    }
}
