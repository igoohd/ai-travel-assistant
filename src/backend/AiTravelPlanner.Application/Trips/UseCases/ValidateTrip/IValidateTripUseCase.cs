namespace AiTravelPlanner.Application.Trips.UseCases.ValidateTrip;

public interface IValidateTripUseCase
{
    Task<ValidateTripResult> HandleAsync(ValidateTripCommand command, CancellationToken cancellationToken);
}
