using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

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
            Interests: request.Interests);
    }

    public static GenerateTripResponse ToResponse(this Plan tripPlan)
    {
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
                        EstimatedCost: activity.EstimatedCost))
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
            Destination: tripPlan.Destination,
            NumberOfDays: tripPlan.NumberOfDays,
            Overview: tripPlan.Overview,
            Days: days,
            Budget: new BudgetEstimateResponse(
                Hotel: tripPlan.Budget.Hotel,
                Transportation: tripPlan.Budget.Transportation,
                Food: tripPlan.Budget.Food,
                Activities: tripPlan.Budget.Activities,
                Total: tripPlan.Budget.Total,
                Category: tripPlan.Budget.Category,
                Currency: tripPlan.Budget.Currency),
            Highlights: tripPlan.Highlights,
            TravelTips: tripPlan.TravelTips,
            ValidationIssues: tripPlan.ValidationIssues
                .Select(issue => new ValidationIssueResponse(
                    Code: issue.Code,
                    Message: issue.Message,
                    Severity: issue.Severity))
                .ToArray());
    }
}
