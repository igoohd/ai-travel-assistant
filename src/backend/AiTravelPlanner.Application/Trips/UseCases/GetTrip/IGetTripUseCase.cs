namespace AiTravelPlanner.Application.Trips.UseCases.GetTrip;

public interface IGetTripUseCase
{
    Task<GetTripResult> HandleAsync(
        GetTripQuery query,
        CancellationToken cancellationToken);
}
