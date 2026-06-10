namespace AiTravelPlanner.Api.Contracts.Trips.TripDetails;

public sealed record RestaurantSuggestionResponse(
    string Name,
    string Cuisine,
    string Notes,
    decimal EstimatedCost);
