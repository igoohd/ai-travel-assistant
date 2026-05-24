namespace AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;

public sealed record GenerateTripResponse(
    string Destination,
    int NumberOfDays,
    string Overview,
    IReadOnlyCollection<TripDayResponse> Days,
    BudgetEstimateResponse Budget,
    IReadOnlyCollection<string> Highlights,
    IReadOnlyCollection<string> TravelTips,
    IReadOnlyCollection<TripValidationIssueResponse> ValidationIssues
    );

public sealed record TripDayResponse(
int DayNumber,
string Title,
string Description,
IReadOnlyCollection<TripActivityResponse> Activities,
IReadOnlyCollection<RestaurantSuggestionResponse> Restaurants);

public sealed record TripActivityResponse(
    string TimeOfDay,
    string Title,
    string Description,
    decimal EstimatedCost
);

public sealed record BudgetEstimateResponse(
    decimal Hotel,
    decimal Transportation,
    decimal Food,
    decimal Activities,
    decimal Total,
    string Category);

public sealed record RestaurantSuggestionResponse(
    string Name,
    string Cuisine,
    string Notes,
    decimal EstimatedCost
);

public sealed record TripValidationIssueResponse(
    string Code,
    string Message,
    string Severity
);