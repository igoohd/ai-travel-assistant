using AiTravelPlanner.Application.Trips.Prompting;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;

namespace AiTravelPlanner.Application.Tests.Trips.Prompting;

public sealed class TripGenerationPromptBuilderTests
{
    [Fact]
    public void Build_IncludesTripRequestValuesAndAdditionalInstruction()
    {
        var builder = new TripGenerationPromptBuilder();
        var command = new GenerateTripCommand(
            Destination: "Tokyo",
            NumberOfDays: 5,
            Budget: 2_000m,
            Currency: "USD",
            Interests: ["food", "technology"]);

        var prompt = builder.Build(
            command,
            "Generate a cheaper version.");

        Assert.Contains("Tokyo", prompt);
        Assert.Contains("5", prompt);
        Assert.Contains("2000", prompt);
        Assert.Contains("USD", prompt);
        Assert.Contains("food, technology", prompt);
        Assert.Contains("Generate a cheaper version.", prompt);
        Assert.Contains("Return only valid JSON.", prompt);
    }

    [Fact]
    public void BuildSystemPrompt_ReturnsTravelPlanningInstruction()
    {
        var builder = new TripGenerationPromptBuilder();

        var prompt = builder.BuildSystemPrompt();

        Assert.Contains("travel planning assistant", prompt);
        Assert.Contains("personalized travel itineraries", prompt);
    }
}
