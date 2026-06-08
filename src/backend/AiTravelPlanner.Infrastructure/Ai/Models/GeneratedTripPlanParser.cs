using System.Text.Json;

namespace AiTravelPlanner.Infrastructure.Ai.Models;

public sealed class GeneratedTripPlanParser
{
    public static GeneratedTripPlan Parse(string content, string providerName)
    {
        try
        {
            var trimmedContent = content.Trim();

            if (trimmedContent.StartsWith("```"))
            {
                var firstNewLineIndex = trimmedContent.IndexOf('\n');
                var lastFenceIndex = trimmedContent.LastIndexOf("```", StringComparison.Ordinal);

                if (firstNewLineIndex >= 0 && lastFenceIndex > firstNewLineIndex)
                {
                    trimmedContent = trimmedContent[
                        (firstNewLineIndex + 1)..lastFenceIndex
                    ].Trim();
                }
            }

            var generatedPlan = JsonSerializer.Deserialize<GeneratedTripPlan>(
                trimmedContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (generatedPlan is null)
            {
                throw new InvalidOperationException($"{providerName} returned an invalid trip plan.");
            }

            return generatedPlan;
        }
        catch (JsonException exception)
        {
            throw new InvalidOperationException(
                $"{providerName} returned a response that could not be parsed as a trip plan JSON object.",
                exception);
        }
    }
}