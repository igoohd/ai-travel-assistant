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

    public async Task<GenerateTripResult> HandleAsync(GenerateTripCommand command)
    {
        if (command.NumberOfDays <= 0)
        {
            return GenerateTripResult.Failure(
            [
                "Number of days must be greater than zero."
            ]);
        }

        var plan = await _tripPlanGenerator.GenerateAsync(command);
        var validationIssues = _tripPlanValidator.Validate(plan, command);

        if (validationIssues.Any(issue => issue.Code == ValidationIssueCodes.BudgetExceeded))
        {
            plan = await _tripPlanGenerator.GenerateAsync(
                command,
                "The previous plan exceeded the budget. Generate a cheaper version with lower activity and restaurant costs.");

            validationIssues = _tripPlanValidator.Validate(plan, command);
        }

        return GenerateTripResult.Success(plan, validationIssues);
    }
}
