namespace AiTravelPlanner.Domain.Trips;

public sealed record RestaurantSuggestion(
    string Name,
    string Cuisine,
    string Notes,
    decimal EstimatedCost
);
