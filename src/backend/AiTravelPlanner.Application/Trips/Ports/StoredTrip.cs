using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.Ports;

public sealed record StoredTrip(
    Plan Plan,
    GenerateTripCommand Command,
    int RetryCount,
    IReadOnlyList<string> RetryReasons,
    int DurationMs,
    IReadOnlyList<ValidationIssue> ValidationIssues);