namespace AiTravelPlanner.Api.Contracts.Trips.TripDetails;

public sealed record GenerationDiagnosticsResponse(
    int RetryCount,
    IReadOnlyList<string> RetryReasons,
    int DurationMs);
