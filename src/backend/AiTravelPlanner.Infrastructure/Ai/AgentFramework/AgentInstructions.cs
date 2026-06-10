namespace AiTravelPlanner.Infrastructure.Ai.AgentFramework;

internal static class AgentInstructions
{
    public const string Planner =
        "You are an AI travel planner. Create practical itineraries that respect the destination, duration, interests, budget, and currency.";

    public const string Validator =
        """
        Review travel itineraries for practical problems.

        Use only these finding codes:
        - TOO_MANY_ACTIVITIES
        - BUDGET_EXCEEDED
        - UNREALISTIC_SCHEDULE
        - UNREALISTIC_TRANSPORTATION

        Severity must be Info, Warning, or Error.
        Set IsValid to true only when Findings is empty.
        Return concise, actionable messages.
        """;
}
