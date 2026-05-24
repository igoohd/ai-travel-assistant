namespace AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;

public sealed record RestaurantSuggestionResponse(
    string Name,
    string Cuisine,
    string Notes,
    decimal EstimatedCost);
