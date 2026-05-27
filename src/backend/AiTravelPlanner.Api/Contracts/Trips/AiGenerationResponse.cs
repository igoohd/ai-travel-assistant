namespace AiTravelPlanner.Api.Contracts.Trips;

public sealed record AiGenerationResponse(
    string Provider,
    string Model);