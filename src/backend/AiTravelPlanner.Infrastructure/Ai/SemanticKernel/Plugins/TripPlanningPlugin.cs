using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace AiTravelPlanner.Infrastructure.Ai.SemanticKernel.Plugins;

public sealed class TripPlanningPlugin
{
    [KernelFunction("calculate_daily_budget")]
    [Description("Calculates the average available budget per travel day.")]
    public decimal CalculateDailyBudget(
        [Description("The total trip budget.")] decimal totalBudget,
        [Description("The number of travel days.")] int numberOfDays)
    {
        if (numberOfDays <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfDays));
        }

        return decimal.Round(totalBudget / numberOfDays, 2);
    }
}