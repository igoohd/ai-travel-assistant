using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.GetTrip;

public sealed record GetTripResult(Plan? Plan)
{
    public bool IsFound => Plan is not null;
}