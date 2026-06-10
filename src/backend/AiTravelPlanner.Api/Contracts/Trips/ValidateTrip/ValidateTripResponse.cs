namespace AiTravelPlanner.Api.Contracts.Trips.ValidateTrip;

public sealed record ValidateTripResponse(IReadOnlyList<ValidationIssueResponse> ValidationIssues);
