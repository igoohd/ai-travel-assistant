using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public sealed class GenerateTripHandler : IGenerateTripUseCase
{
    public TripPlan Handle(GenerateTripCommand command)
    {
        var days = Enumerable.Range(1, command.NumberOfDays)
            .Select(day => new TripDay(
                DayNumber: day,
                Title: $"Day {day} in {command.Destination}",
                Description: "A placeholder day plan."))
            .ToArray();

        return new TripPlan(
            Destination: command.Destination,
            NumberOfDays: command.NumberOfDays,
            Overview: $"A {command.NumberOfDays}-day trip to {command.Destination}.",
            Days: days);
    }
}