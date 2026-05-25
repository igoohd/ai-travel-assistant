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

    public GenerateTripResult Handle(GenerateTripCommand command)
    {
        if (command.NumberOfDays <= 0)
        {
            return GenerateTripResult.Failure(
            [
                "Number of days must be greater than zero."
            ]);
        }

        var plan = _tripPlanGenerator.Generate(command);
        var validationIssues = _tripPlanGenerator.Validate(plan, command);

        return GenerateTripResult.Success(plan with
        {
            ValidationIssues = validationIssues
        });

    }
}
