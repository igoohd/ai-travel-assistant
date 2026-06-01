using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;

namespace AiTravelPlanner.Application.Trips.Prompting;

public interface ITripGenerationPromptBuilder
{
    string Build(
        GenerateTripCommand command,
        string? additionalInstruction = null);

    string BuildSystemPrompt();
}