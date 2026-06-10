using System.Text.Json;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;
using AiTravelPlanner.Infrastructure.Ai.AgentFramework.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.DependencyInjection;

namespace AiTravelPlanner.Infrastructure.Ai.AgentFramework;

public sealed class AgentFrameworkTripPlanReviewer
{
    private readonly ChatClientAgent _validatorAgent;

    public AgentFrameworkTripPlanReviewer(
        [FromKeyedServices(AgentKeys.Validator)]
        ChatClientAgent validatorAgent)
    {
        _validatorAgent = validatorAgent;
    }

    public async Task<AgentReviewResult> ReviewAsync(
    Plan plan,
    GenerateTripCommand command,
    CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Serialize(new
        {
            Request = new
            {
                command.Destination,
                command.NumberOfDays,
                command.Budget,
                command.Currency,
                command.Interests
            },
            Plan = plan
        });

        var prompt = $"""
        Review this trip request and generated itinerary.

        {input}
        """;

        var response =
            await _validatorAgent.RunAsync<AgentValidationResult>(
                prompt,
                cancellationToken: cancellationToken);

        return new AgentReviewResult(
            response.Result.ToValidationIssues(),
            response.Usage);
    }
}