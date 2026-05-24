using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Application.Trips.Services;
using AiTravelPlanner.Domain.Trips;

public class StubTripPlanGenerator : ITripPlanGenerator
{
    public TripPlan Generate(GenerateTripCommand command)
    {
        if (command.NumberOfDays <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(command.NumberOfDays),
                "Number of days must be greater than zero.");
        }

        var interests = command.Interests
            .Where(interest => !string.IsNullOrWhiteSpace(interest))
            .Select(interest => interest.Trim())
            .ToArray();

        var fallbackInterest = interests.FirstOrDefault() ?? "local culture";

        var dailyBudget = command.Budget / command.NumberOfDays;
        var activityCost = Math.Round(dailyBudget * 0.12m, 2);
        var restaurantCost = Math.Round(dailyBudget * 0.15m, 2);

        var days = Enumerable.Range(1, command.NumberOfDays)
            .Select(day =>
            {
                var theme = interests.Length == 0
                ? fallbackInterest
                : interests[(day - 1) % interests.Length];

                return new TripDay(
                    DayNumber: day,
                    Title: $"Day {day} in {command.Destination}",
                    Description: "A placeholder day plan.",
                    Activities:
                    [
                        new TripActivity(
                            TimeOfDay: "Morning",
                            Title: $"{command.Destination} orientation walk",
                            Description: "Start with a relaxed walk through a central neighborhood.",
                            EstimatedCost: activityCost),
                        new TripActivity(
                            TimeOfDay: "Afternoon",
                            Title: $"{theme} experience",
                            Description: $"Explore a recommended place or activity connected to {theme}.",
                            EstimatedCost: activityCost),
                        new TripActivity(
                            TimeOfDay: "Evening",
                            Title: "Local dinner area",
                            Description: "End the day near a lively food or entertainment district.",
                            EstimatedCost: activityCost)
                    ],
                    Restaurants:
                    [
                        new RestaurantSuggestion(
                            Name: $"{command.Destination} local favorite",
                            Cuisine: "Local",
                            Notes: $"A placeholder restaurant suggestion for a {theme}-focused day.",
                            EstimatedCost: restaurantCost)
                    ]);
            }).ToArray();

        var budget = new BudgetEstimate(
            Hotel: Math.Round(command.Budget * 0.40m, 2),
            Transportation: Math.Round(command.Budget * 0.15m, 2),
            Food: Math.Round(command.Budget * 0.25m, 2),
            Activities: Math.Round(command.Budget * 0.20m, 2),
            Total: command.Budget,
            Category: ClassifyBudget(command.Budget, command.NumberOfDays));

        return new TripPlan(
            Destination: command.Destination,
            NumberOfDays: command.NumberOfDays,
            Overview: $"A {command.NumberOfDays}-day trip to {command.Destination}.",
            Days: days,
            Budget: budget,
            Highlights:
            [
                $"Explore {command.Destination} through {string.Join(", ", interests.DefaultIfEmpty(fallbackInterest))}.",
                "Balance planned activities with flexible discovery time.",
                "Use the daily themes to keep the itinerary focused."
            ],
            TravelTips:
            [
                "Book popular restaurants and attractions in advance.",
                "Group nearby activities together to reduce transit time.",
                "Keep a small contingency budget for local transportation."
            ]
        );
    }

    private static string ClassifyBudget(decimal budget, int numberOfDays)
    {
        var dailyBudget = budget / numberOfDays;

        return dailyBudget switch
        {
            < 100 => "budget",
            < 300 => "mid-range",
            _ => "luxury"
        };
    }
}