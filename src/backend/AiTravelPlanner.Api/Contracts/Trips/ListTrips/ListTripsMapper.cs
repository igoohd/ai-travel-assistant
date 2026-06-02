using AiTravelPlanner.Application.Trips.UseCases.ListTrips;

namespace AiTravelPlanner.Api.Contracts.Trips.ListTrips;

public static class ListTripsMapper
{
    public static ListTripsResponse ToResponse(this ListTripsResult result)
    {
        var trips = result.Trips
            .Select(trip => new TripListItemResponse(
                Id: trip.Plan.Id,
                CreatedAt: trip.Plan.CreatedAt,
                Destination: trip.Plan.Destination,
                NumberOfDays: trip.Plan.NumberOfDays,
                EstimatedTotal: trip.Plan.Budget.Total,
                Currency: trip.Plan.Budget.Currency.Value,
                BudgetCategory: trip.Plan.Budget.Category,
                AiMetadata: new AiGenerationResponse(
                    Provider: trip.Plan.AiMetadata.Provider,
                    Model: trip.Plan.AiMetadata.Model,
                    PromptTokens: trip.Plan.AiMetadata.PromptTokens,
                    CompletionTokens: trip.Plan.AiMetadata.CompletionTokens,
                    TotalTokens: trip.Plan.AiMetadata.TotalTokens),
                ValidationIssueCount: trip.ValidationIssues.Count))
            .ToArray();

        return new ListTripsResponse(trips);
    }
}
