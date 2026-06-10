using System.Text.Json;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;
using AiTravelPlanner.Infrastructure.Ai.AgentFramework.Contracts;
using AiTravelPlanner.Infrastructure.Ai.Chat;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AiTravelPlanner.Infrastructure.Ai.AgentFramework;

public sealed class AgentFrameworkTripPlanReviewer
{
    private readonly ChatClientAgent _validatorAgent;
    private readonly GitHubModelsChatOptions _options;

    public AgentFrameworkTripPlanReviewer(
        [FromKeyedServices(AgentKeys.Validator)]
        ChatClientAgent validatorAgent,
        IOptions<GitHubModelsChatOptions> options)
    {
        _validatorAgent = validatorAgent;
        _options = options.Value;
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
            Plan = new
            {
                plan.Destination,
                plan.NumberOfDays,
                plan.Days,
                plan.Budget,
                plan.Summary
            }
        });

        var prompt = $"""
        Review this trip request and generated itinerary.

        {input}
        """;

        var runOptions = new ChatClientAgentRunOptions(
            new ChatOptions
            {
                Temperature = (float)_options.Temperature,
                MaxOutputTokens = _options.MaxTokens
            });

        var response = await _validatorAgent.RunAsync<AgentValidationResult>(
            prompt,
            options: runOptions,
            cancellationToken: cancellationToken);

        return new AgentReviewResult(
            response.Result.ToValidationIssues(),
            response.Usage);
    }
}
