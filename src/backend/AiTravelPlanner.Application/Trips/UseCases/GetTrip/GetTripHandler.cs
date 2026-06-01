using AiTravelPlanner.Application.Trips.Ports;

namespace AiTravelPlanner.Application.Trips.UseCases.GetTrip;

public sealed class GetTripHandler : IGetTripUseCase
{
    private readonly ITripPlanRepository _tripPlanRepository;

    public GetTripHandler(ITripPlanRepository tripPlanRepository)
    {
        _tripPlanRepository = tripPlanRepository;
    }

    public async Task<GetTripResult> HandleAsync(
        GetTripQuery query,
        CancellationToken cancellationToken)
    {
        var plan = await _tripPlanRepository.GetByIdAsync(
            query.Id,
            cancellationToken);

        return new GetTripResult(plan);
    }
}
