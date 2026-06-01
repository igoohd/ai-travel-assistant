namespace AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;

public sealed record GenerationDiagnosticsResponse(
    int RetryCount,
    IReadOnlyList<string> RetryReasons);
