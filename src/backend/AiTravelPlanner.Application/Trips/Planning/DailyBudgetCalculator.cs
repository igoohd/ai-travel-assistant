namespace AiTravelPlanner.Application.Trips.Planning;

public sealed class DailyBudgetCalculator
{
    public decimal Calculate(decimal totalBudget, int numberOfDays)
    {
        if (totalBudget < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalBudget));
        }

        if (numberOfDays <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfDays));
        }

        return decimal.Round(totalBudget / numberOfDays, 2);
    }
}
