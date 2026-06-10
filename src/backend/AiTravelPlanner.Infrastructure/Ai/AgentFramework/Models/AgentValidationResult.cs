namespace AiTravelPlanner.Infrastructure.Ai.AgentFramework.Models;

public sealed record AgentValidationResult(
    bool IsValid,
    IReadOnlyList<AgentValidationFinding> Findings);

public sealed record AgentValidationFinding(
    string Code,
    string Message,
    string Severity);