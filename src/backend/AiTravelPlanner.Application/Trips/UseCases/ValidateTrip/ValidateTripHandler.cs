using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.Validation;

namespace AiTravelPlanner.Application.Trips.UseCases.ValidateTrip;

public class ValidateTripHandler : IValidateTripUseCase
{
    private readonly ITripPlanRepository _tripPlanRepository;
    private readonly ITripPlanValidator _tripPlanValidator;

    public ValidateTripHandler(ITripPlanRepository tripPlanRepository, ITripPlanValidator tripPlanValidator)
    {
        _tripPlanRepository = tripPlanRepository;
        _tripPlanValidator = tripPlanValidator;
    }

    public async Task<ValidateTripResult> HandleAsync(ValidateTripCommand command, CancellationToken cancellationToken)
    {
        var plan = await _tripPlanRepository.GetByIdAsync(
            command.TripId,
            cancellationToken);

        if (plan is null)
        {
            return ValidateTripResult.Failure(["Trip not found."]);
        }

        var issues = _tripPlanValidator.Validate(plan);

        return ValidateTripResult.Success(issues);
    }
}
