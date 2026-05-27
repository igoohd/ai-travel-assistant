using AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;

namespace AiTravelPlanner.Api.Contracts.Trips.ValidateTrip;

public sealed record ValidateTripResponse(IReadOnlyList<ValidationIssueResponse> ValidationIssues);