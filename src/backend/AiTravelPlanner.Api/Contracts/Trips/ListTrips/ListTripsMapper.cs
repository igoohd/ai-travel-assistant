using AiTravelPlanner.Api.Contracts.Trips.ListTrips;

namespace AiTravelPlanner.Application.Trips.ListTrips;

public static class ListTripsMapper
{
    public static ListTripsResponse ToResponse(this ListTripsResult result)
    {
        var trips = result.Plans
            .Select(plan => new TripListItemResponse(
                Id: plan.Id,
                CreatedAt: plan.CreatedAt,
                Destination: plan.Destination,
                NumberOfDays: plan.NumberOfDays,
                EstimatedTotal: plan.Budget.Total,
                Currency: plan.Budget.Currency.Value,
                BudgetCategory: plan.Budget.Category))
            .ToArray();

        return new ListTripsResponse(trips);
    }
}