using AiTravelPlanner.Application.Trips.UseCases.ValidateTrip;

namespace AiTravelPlanner.Api.Contracts.Trips.ValidateTrip;

public static class ValidateTripMapper
{
    public static ValidateTripResponse ToResponse(this ValidateTripResult result)
    {
        var validationIssues = result.ValidationIssues
            .Select(issue => new ValidationIssueResponse(
                    Code: issue.Code,
                    Severity: issue.Severity.ToString(),
                    Message: issue.Message))
            .ToArray();

        return new ValidateTripResponse(validationIssues);
    }
}
