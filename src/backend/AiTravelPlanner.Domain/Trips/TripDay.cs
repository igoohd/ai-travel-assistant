namespace AiTravelPlanner.Domain.Trips;

public sealed record TripDay(
    int DayNumber,
    string Title,
    string Description,
    IReadOnlyCollection<TripActivity> Activities,
    IReadOnlyCollection<RestaurantSuggestion> Restaurants);
