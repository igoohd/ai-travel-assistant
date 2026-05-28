using AiTravelPlanner.Application.Trips.ListTrips;

namespace AiTravelPlanner.Api.Contracts.Trips.ListTrips;

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
                BudgetCategory: plan.Budget.Category,
                AiMetadata: new AiGenerationResponse(
                    Provider: plan.AiMetadata.Provider,
                    Model: plan.AiMetadata.Model,
                    PromptTokens: plan.AiMetadata.PromptTokens,
                    CompletionTokens: plan.AiMetadata.CompletionTokens,
                    TotalTokens: plan.AiMetadata.TotalTokens)))
            .ToArray();

        return new ListTripsResponse(trips);
    }
}
