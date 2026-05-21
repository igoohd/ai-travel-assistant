using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public sealed class GenerateTripHandler : IGenerateTripUseCase
{
    public TripPlan Handle(GenerateTripCommand command)
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
                            Description: "Start with a relaxed walk through a central neighborhood."),
                        new TripActivity(
                            TimeOfDay: "Afternoon",
                            Title: $"{theme} experience",
                            Description: $"Explore a recommended place or activity connected to {theme}."),
                        new TripActivity(
                            TimeOfDay: "Evening",
                            Title: "Local dinner area",
                            Description: "End the day near a lively food or entertainment district.")
                    ]);
            }).ToArray();

        return new TripPlan(
            Destination: command.Destination,
            NumberOfDays: command.NumberOfDays,
            Overview: $"A {command.NumberOfDays}-day trip to {command.Destination}.",
            Days: days);
    }
}