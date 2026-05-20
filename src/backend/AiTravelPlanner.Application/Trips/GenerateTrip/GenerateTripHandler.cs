using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public sealed class GenerateTripHandler : IGenerateTripUseCase
{
    public TripPlan Handle(GenerateTripCommand command)
    {
        return new TripPlan(
            Destination: command.Destination,
            NumberOfDays: command.NumberOfDays,
            Overview: $"A {command.NumberOfDays}-day trip to {command.Destination}.");
    }
}