using AiTravelPlanner.Api.Contracts.Trips.TripDetails;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;

namespace AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;

public static class GenerateTripMapper
{
    public static GenerateTripCommand ToCommand(this GenerateTripRequest request)
    {
        return new GenerateTripCommand(
            Destination: request.Destination,
            NumberOfDays: request.NumberOfDays,
            Budget: request.Budget,
            Currency: request.Currency,
            Interests: request.Interests
                .Where(interest => !string.IsNullOrWhiteSpace(interest))
                .Select(interest => interest.Trim())
                .ToArray());
    }

    public static TripDetailsResponse ToResponse(this GenerateTripResult result)
    {
        var plan = result.Plan
            ?? throw new InvalidOperationException(
                "Cannot map a failed trip generation result to a response.");

        return TripDetailsMapper.ToResponse(
            plan,
            result.ValidationIssues,
            result.RetryCount,
            result.RetryReasons,
            result.DurationMs);
    }
}
