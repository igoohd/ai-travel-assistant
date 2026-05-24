namespace AiTravelPlanner.Domain.Trips;

public sealed record Day(
    int DayNumber,
    string Title,
    string Description,
    IReadOnlyCollection<Activity> Activities,
    IReadOnlyCollection<RestaurantSuggestion> Restaurants);
