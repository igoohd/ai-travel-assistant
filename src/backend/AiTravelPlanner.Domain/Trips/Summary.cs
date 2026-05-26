public sealed record Summary(
    string Overview,
    IReadOnlyList<string> Highlights,
    IReadOnlyList<string> TravelTips
);