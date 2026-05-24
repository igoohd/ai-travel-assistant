namespace AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;

public sealed record DayResponse(
    int DayNumber,
    string Title,
    string Description,
    IReadOnlyCollection<ActivityResponse> Activities,
    IReadOnlyCollection<RestaurantSuggestionResponse> Restaurants);
