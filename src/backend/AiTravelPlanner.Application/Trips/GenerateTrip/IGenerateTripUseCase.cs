using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public interface IGenerateTripUseCase
{
    Plan Handle(GenerateTripCommand command);
}
