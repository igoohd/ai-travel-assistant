namespace AiTravelPlanner.Application.Trips.UseCases.ListTrips;

public interface IListTripsUseCase
{
    Task<ListTripsResult> HandleAsync(
        ListTripsQuery query,
        CancellationToken cancellationToken);
}
