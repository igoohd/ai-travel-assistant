using AiTravelPlanner.Application.Trips.ValidateTrip;

public interface IValidateTripUseCase
{
    Task<ValidateTripResult> HandleAsync(ValidateTripCommand command, CancellationToken cancellationToken);
}