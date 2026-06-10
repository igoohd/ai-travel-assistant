namespace AiTravelPlanner.Api.Contracts.Trips.TripDetails;

public sealed record SummaryResponse(
    string Overview,
    IReadOnlyCollection<string> Highlights,
    IReadOnlyCollection<string> TravelTips);
