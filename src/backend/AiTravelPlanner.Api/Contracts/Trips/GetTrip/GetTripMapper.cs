using AiTravelPlanner.Api.Contracts.Trips.TripDetails;
using AiTravelPlanner.Application.Trips.UseCases.GetTrip;

namespace AiTravelPlanner.Api.Contracts.Trips.GetTrip;

public static class GetTripMapper
{
    public static TripDetailsResponse ToResponse(this GetTripResult result)
    {
        var storedTrip = result.Trip
            ?? throw new InvalidOperationException(
                "Cannot map a missing trip to a response.");

        return TripDetailsMapper.ToResponse(
            storedTrip.Plan,
            storedTrip.ValidationIssues,
            storedTrip.RetryCount,
            storedTrip.RetryReasons,
            storedTrip.DurationMs);
    }
}
