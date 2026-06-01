using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.Validation;

public interface ITripPlanValidator
{
    IReadOnlyList<ValidationIssue> Validate(Plan plan, GenerateTripCommand command);
}
