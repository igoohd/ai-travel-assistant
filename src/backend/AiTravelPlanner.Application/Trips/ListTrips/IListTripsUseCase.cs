namespace AiTravelPlanner.Application.Trips.ListTrips;

public interface IListTripUseCase
{
    Task<ListTripsResult> HandleAsync(
        ListTripsQuery query,
        CancellationToken cancellationToken);
}