using AiTravelPlanner.Domain.Trips;
using AiTravelPlanner.Infrastructure.Ai.AgentFramework.Models;

namespace AiTravelPlanner.Infrastructure.Ai.AgentFramework;

public static class AgentValidationMapper
{
    private static readonly HashSet<string> AllowedCodes =
    [
        ValidationIssueCodes.TooManyActivities,
        ValidationIssueCodes.BudgetExceeded,
        ValidationIssueCodes.UnrealisticSchedule,
        ValidationIssueCodes.UnrealisticTransportation
    ];

    public static IReadOnlyList<ValidationIssue> ToValidationIssues(
        this AgentValidationResult result)
    {
        return result.Findings
            .Where(finding =>
                AllowedCodes.Contains(finding.Code) &&
                !string.IsNullOrWhiteSpace(finding.Message) &&
                Enum.TryParse<ValidationSeverity>(
                    finding.Severity,
                    ignoreCase: true,
                    out _))
            .Select(finding => new ValidationIssue(
                finding.Code,
                finding.Message.Trim(),
                Enum.Parse<ValidationSeverity>(
                    finding.Severity,
                    ignoreCase: true)))
            .ToArray();
    }
}