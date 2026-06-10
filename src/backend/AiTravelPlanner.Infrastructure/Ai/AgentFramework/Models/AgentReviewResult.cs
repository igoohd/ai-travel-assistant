using AiTravelPlanner.Domain.Trips;
using Microsoft.Extensions.AI;

namespace AiTravelPlanner.Infrastructure.Ai.AgentFramework.Models;

public sealed record AgentReviewResult(
    IReadOnlyList<ValidationIssue> Issues,
    UsageDetails? Usage);