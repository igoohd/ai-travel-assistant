using AiTravelPlanner.Application.Trips.Ports;

namespace AiTravelPlanner.Application.Trips.UseCases.ListTrips;

public sealed class ListTripsHandler : IListTripsUseCase
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

        return new ListTripsResult(trips.Select(st => st.Plan).ToArray());
    }
}
