using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.Ports;

public sealed record StoredTrip(
    Plan Plan,
    GenerateTripCommand Command,
    IReadOnlyList<ValidationIssue> ValidationIssues);