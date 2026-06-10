using System.ComponentModel;
using AiTravelPlanner.Application.Trips.Planning;
using Microsoft.SemanticKernel;

namespace AiTravelPlanner.Infrastructure.Ai.SemanticKernel.Plugins;

public sealed class TripPlanningPlugin(DailyBudgetCalculator dailyBudgetCalculator)
{
    [KernelFunction("calculate_daily_budget")]
    [Description("Calculates the average available budget per travel day.")]
    public decimal CalculateDailyBudget(
        [Description("The total trip budget.")] decimal totalBudget,
        [Description("The number of travel days.")] int numberOfDays)
    {
        return dailyBudgetCalculator.Calculate(totalBudget, numberOfDays);
    }
}