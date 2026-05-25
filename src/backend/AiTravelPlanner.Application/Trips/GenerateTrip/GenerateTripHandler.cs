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
        var plan = _tripPlanGenerator.Generate(command);
        var validationIssues = _tripPlanGenerator.Validate(plan, command);

        return plan with
        {
            ValidationIssues = validationIssues
        };
    }
}
