using AiTravelPlanner.Application.Trips.Ports;

namespace AiTravelPlanner.Application.Trips.UseCases.GetTrip;

public sealed record GetTripResult(StoredTrip? Trip)
{
    public bool IsFound => Trip is not null;
}
