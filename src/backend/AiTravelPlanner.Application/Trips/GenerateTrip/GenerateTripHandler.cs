using AiTravelPlanner.Application.Trips.Services;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public sealed class GenerateTripHandler : IGenerateTripUseCase
{
    private readonly ITripPlanGenerator _tripPlanGenerator;
    private readonly ITripPlanValidator _tripPlanValidator;

    public GenerateTripHandler(
        ITripPlanGenerator tripPlanGenerator,
        ITripPlanValidator tripPlanValidator)
    {
        _tripPlanGenerator = tripPlanGenerator;
        _tripPlanValidator = tripPlanValidator;
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
        var validationIssues = _tripPlanValidator.Validate(plan);

        return GenerateTripResult.Success(plan, validationIssues);
    }
}
