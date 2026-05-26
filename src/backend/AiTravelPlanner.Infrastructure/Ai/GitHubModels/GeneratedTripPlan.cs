namespace AiTravelPlanner.Infrastructure.Ai.GitHubModels;

public sealed record GeneratedTripPlan(
    string Overview,
    IReadOnlyList<GeneratedDay> Days,
    IReadOnlyList<string> Highlights,
    IReadOnlyList<string> TravelTips);

public sealed record GeneratedDay(
    int DayNumber,
    string Title,
    string Description,
    IReadOnlyList<GeneratedActivity> Activities,
    IReadOnlyList<GeneratedRestaurant> Restaurants);

public sealed record GeneratedActivity(
    string TimeOfDay,
    string Title,
    string Description,
    decimal EstimatedCost,
    decimal DurationHours,
    decimal TransitMinutesFromPrevious);

public sealed record GeneratedRestaurant(
    string Name,
    string Cuisine,
    string Notes,
    decimal EstimatedCost);