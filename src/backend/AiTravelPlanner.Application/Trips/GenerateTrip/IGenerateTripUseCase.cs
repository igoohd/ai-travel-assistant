using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public interface IGenerateTripUseCase
{
    TripPlan Handle(GenerateTripCommand command);
}