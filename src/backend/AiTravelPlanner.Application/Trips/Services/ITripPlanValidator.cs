using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.Services;

public interface ITripPlanValidator
{
    IReadOnlyList<ValidationIssue> Validate(Plan plan);
}
