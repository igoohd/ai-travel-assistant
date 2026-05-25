using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Trips.Services;

public interface ITripPlanGenerator
{
    Plan Generate(GenerateTripCommand command);
    IReadOnlyList<ValidationIssue> Validate(Plan plan, GenerateTripCommand command);
}
