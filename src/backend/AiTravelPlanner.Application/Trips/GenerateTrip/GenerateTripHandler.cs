using AiTravelPlanner.Application.Trips.Services;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public sealed class GenerateTripHandler : IGenerateTripUseCase
{

    private readonly ITripPlanGenerator _tripPlanGenerator;

    public GenerateTripHandler(ITripPlanGenerator tripPlanGenerator)
    {
        _tripPlanGenerator = tripPlanGenerator;
    }

    public Plan Handle(GenerateTripCommand command)
    {
        return _tripPlanGenerator.Generate(command);
    }
}
