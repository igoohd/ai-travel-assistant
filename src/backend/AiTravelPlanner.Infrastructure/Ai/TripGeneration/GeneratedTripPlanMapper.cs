using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Infrastructure.Ai.TripGeneration;

public static class GeneratedTripPlanMapper
{
    public static Plan ToPlan(this GeneratedTripPlan generatedPlan, GenerateTripCommand command, AiGenerationMetadata aiMetadata)
    {
        var currency = new CurrencyCode(command.Currency);

        var activityTotal = generatedPlan.Days
            .SelectMany(day => day.Activities)
            .Sum(activity => activity.EstimatedCost);

        var foodTotal = generatedPlan.Days
            .SelectMany(day => day.Restaurants)
            .Sum(restaurant => restaurant.EstimatedCost);

        var hotelTotal = Math.Round(command.Budget * 0.40m, 2);
        var transportationTotal = Math.Round(command.Budget * 0.15m, 2);

        var estimatedTotal = hotelTotal + transportationTotal + foodTotal + activityTotal;

        return new Plan(
            Id: Guid.NewGuid(),
            CreatedAt: DateTimeOffset.UtcNow,
            AiMetadata: aiMetadata,
            Destination: command.Destination,
            NumberOfDays: command.NumberOfDays,
            Days: generatedPlan.Days
                .Select(day => new Day(
                    DayNumber: day.DayNumber,
                    Title: day.Title,
                    Description: day.Description,
                    Activities: day.Activities
                        .Select(activity => new Activity(
                            TimeOfDay: activity.TimeOfDay,
                            Title: activity.Title,
                            Description: activity.Description,
                            EstimatedCost: activity.EstimatedCost,
                            DurationHours: activity.DurationHours,
                            TransitMinutesFromPrevious: activity.TransitMinutesFromPrevious))
                        .ToArray(),
                    Restaurants: day.Restaurants
                        .Select(restaurant => new RestaurantSuggestion(
                            Name: restaurant.Name,
                            Cuisine: restaurant.Cuisine,
                            Notes: restaurant.Notes,
                            EstimatedCost: restaurant.EstimatedCost))
                        .ToArray()))
                .ToArray(),
            Budget: new BudgetEstimate(
                Hotel: hotelTotal,
                Transportation: transportationTotal,
                Food: foodTotal,
                Activities: activityTotal,
                Total: estimatedTotal,
                Currency: currency,
                Category: ClassifyBudget(estimatedTotal, command.NumberOfDays)),
            Summary: new Summary(
                Highlights: generatedPlan.Highlights,
                TravelTips: generatedPlan.TravelTips,
                Overview: generatedPlan.Overview
            )
        );
    }

    private static string ClassifyBudget(decimal total, int numberOfDays)
    {
        var dailyBudget = total / numberOfDays;

        return dailyBudget switch
        {
            < 100 => "budget",
            < 300 => "mid-range",
            _ => "luxury"
        };
    }
}
