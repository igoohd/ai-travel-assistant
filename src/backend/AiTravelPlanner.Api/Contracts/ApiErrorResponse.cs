namespace AiTravelPlanner.Api.Contracts;

public sealed record ApiErrorResponse(
    IReadOnlyCollection<string> Errors);