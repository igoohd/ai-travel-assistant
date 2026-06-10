namespace AiTravelPlanner.Api.Contracts.Trips.TripDetails;

public sealed record DayResponse(
    int DayNumber,
    string Title,
    string Description,
    IReadOnlyCollection<ActivityResponse> Activities,
    IReadOnlyCollection<RestaurantSuggestionResponse> Restaurants);
