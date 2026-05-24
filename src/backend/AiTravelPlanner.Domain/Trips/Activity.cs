namespace AiTravelPlanner.Domain.Trips;

public sealed record Activity(
    string TimeOfDay,
    string Title,
    string Description,
    decimal EstimatedCost
);
