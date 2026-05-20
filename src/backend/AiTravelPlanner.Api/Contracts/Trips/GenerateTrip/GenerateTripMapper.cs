using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Api.Contracts.Trips;

public static class GenerateTripMapper
{
    public static GenerateTripCommand ToCommand(this GenerateTripRequest request)
    {
        return new GenerateTripCommand(
            Destination: request.Destination,
            NumberOfDays: request.NumberOfDays,
            Budget: request.Budget,
            Interests: request.Interests);
    }

    public static GenerateTripResponse ToResponse(this TripPlan tripPlan)
    {
        return new GenerateTripResponse(
            Destination: tripPlan.Destination,
            NumberOfDays: tripPlan.NumberOfDays,
            Overview: tripPlan.Overview);
    }
}