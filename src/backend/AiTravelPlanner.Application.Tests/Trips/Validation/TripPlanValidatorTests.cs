using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Application.Trips.Validation;
using AiTravelPlanner.Domain.Trips;

namespace AiTravelPlanner.Application.Tests.Trips.Validation;

public sealed class TripPlanValidatorTests
{
    [Fact]
    public void Validate_WithCommand_WhenBudgetIsExceeded_ReturnsBudgetExceededIssue()
    {
        var validator = new TripPlanValidator();
        var plan = CreatePlan(totalBudget: 1_500m);
        var command = new GenerateTripCommand(
            Destination: "Tokyo",
            NumberOfDays: 3,
            Budget: 1_000m,
            Currency: "USD",
            Interests: ["food"]);

        var issues = validator.Validate(plan, command);

        Assert.Contains(
            issues,
            issue => issue.Code == ValidationIssueCodes.BudgetExceeded);
    }

    [Fact]
    public void Validate_WithCommand_WhenBudgetIsNotExceeded_DoesNotReturnBudgetExceededIssue()
    {
        var validator = new TripPlanValidator();
        var plan = CreatePlan(totalBudget: 500m);
        var command = new GenerateTripCommand(
            Destination: "Tokyo",
            NumberOfDays: 3,
            Budget: 1_000m,
            Currency: "USD",
            Interests: ["food"]);

        var issues = validator.Validate(plan, command);

        Assert.DoesNotContain(
            issues,
            issue => issue.Code == ValidationIssueCodes.BudgetExceeded);
    }

    [Fact]
    public void Validate_WhenDayHasMoreThanFourActivities_ReturnsTooManyActivitiesIssue()
    {
        var validator = new TripPlanValidator();
        var plan = CreatePlan(
            totalBudget: 500m,
            activities:
            [
                CreateActivity("Morning"),
                CreateActivity("Late morning"),
                CreateActivity("Afternoon"),
                CreateActivity("Late afternoon"),
                CreateActivity("Evening")
            ]);
        var command = new GenerateTripCommand(
            Destination: "Tokyo",
            NumberOfDays: 3,
            Budget: 1_000m,
            Currency: "USD",
            Interests: ["food"]);

        var issues = validator.Validate(plan, command);

        Assert.Contains(
            issues,
            issue => issue.Code == ValidationIssueCodes.TooManyActivities);
    }

    [Fact]
    public void Validate_WhenDayHasMoreThanTenHoursOfActivities_ReturnsUnrealisticScheduleIssue()
    {
        var validator = new TripPlanValidator();
        var plan = CreatePlan(
            totalBudget: 500m,
            activities:
            [
                CreateActivity("Activity 1", durationHours: 4),
                CreateActivity("Activity 2", durationHours: 4),
                CreateActivity("Activity 3", durationHours: 3)
            ]);
        var command = new GenerateTripCommand(
            Destination: "Tokyo",
            NumberOfDays: 3,
            Budget: 1_000m,
            Currency: "USD",
            Interests: ["food"]);

        var issues = validator.Validate(plan, command);

        Assert.Contains(
            issues,
            issue => issue.Code == ValidationIssueCodes.UnrealisticSchedule);
    }

    [Fact]
    public void Validate_WhenDayHasMoreThanThreeHoursOfTransit_ReturnsUnrealisticTransportationIssue()
    {
        var validator = new TripPlanValidator();
        var plan = CreatePlan(
            totalBudget: 500m,
            activities:
            [
                CreateActivity("Activity 1", transitMinutesFromPrevious: 90),
                CreateActivity("Activity 2", transitMinutesFromPrevious: 90),
                CreateActivity("Activity 3", transitMinutesFromPrevious: 1)
            ]);

        var command = new GenerateTripCommand(
            Destination: "Tokyo",
            NumberOfDays: 3,
            Budget: 1_000m,
            Currency: "USD",
            Interests: ["food"]);

        var issues = validator.Validate(plan, command);

        Assert.Contains(
            issues,
            issue => issue.Code == ValidationIssueCodes.UnrealisticTransportation);
    }

    private static Plan CreatePlan(decimal totalBudget, IReadOnlyList<Activity>? activities = null)
    {
        return new Plan(
            Id: Guid.NewGuid(),
            Destination: "Tokyo",
            NumberOfDays: 3,
            Days:
            [
                new Day(
                    DayNumber: 1,
                    Title: "Arrival",
                    Description: "Arrival day",
                    Activities: activities ?? [],
                    Restaurants: [])
            ],
            Budget: new BudgetEstimate(
                Hotel: totalBudget,
                Transportation: 0,
                Food: 0,
                Activities: 0,
                Total: totalBudget,
                Currency: new CurrencyCode("USD"),
                Category: "Mid-range"),
            Summary: new Summary(
                Overview: "Test summary",
                Highlights: [],
                TravelTips: []),
            CreatedAt: DateTimeOffset.UtcNow,
            AiMetadata: new AiGenerationMetadata(
                Provider: "Test",
                Model: "Test",
                PromptTokens: null,
                CompletionTokens: null,
                TotalTokens: null));
    }

    private static Activity CreateActivity(
        string title,
        decimal durationHours = 2,
        decimal transitMinutesFromPrevious = 30)
    {
        return new Activity(
            Title: title,
            Description: "Test activity",
            EstimatedCost: 100m,
            DurationHours: durationHours,
            TransitMinutesFromPrevious: transitMinutesFromPrevious,
            TimeOfDay: "Morning");
    }
}
