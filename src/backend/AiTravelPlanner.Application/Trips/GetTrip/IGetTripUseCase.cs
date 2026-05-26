using AiTravelPlanner.Application.Trips.GetTrip;

public interface IGetTripUseCase
{
    Task<GetTripResult> HandleAsync(
        GetTripQuery query,
        CancellationToken cancellationToken);
}