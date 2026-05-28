namespace AiTravelPlanner.Application.Trips.ListTrips;

public interface IListTripsUseCase
{
    Task<ListTripsResult> HandleAsync(
        ListTripsQuery query,
        CancellationToken cancellationToken);
}
