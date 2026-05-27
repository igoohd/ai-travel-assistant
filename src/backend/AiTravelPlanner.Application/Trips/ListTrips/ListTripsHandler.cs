using AiTravelPlanner.Application.Trips.Services;

namespace AiTravelPlanner.Application.Trips.ListTrips;

public class ListTripsHandler : IListTripUseCase
{
    private readonly ITripPlanRepository _tripRepository;

    public ListTripsHandler(ITripPlanRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<ListTripsResult> HandleAsync(
        ListTripsQuery query,
        CancellationToken cancellationToken)
    {
        var trips = await _tripRepository.ListAsync(cancellationToken);

        return new ListTripsResult(trips);
    }
}