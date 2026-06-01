using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;

namespace AiTravelPlanner.Application.Trips.Prompting;

public sealed class TripGenerationPromptBuilder : ITripGenerationPromptBuilder
{
    public string Build(GenerateTripCommand command, string? additionalInstruction = null)
    {
        var additionalInstructionText = string.IsNullOrWhiteSpace(additionalInstruction)
            ? "No additional instruction."
            : additionalInstruction;

        return $$"""
        Create a personalized travel itinerary.

        Trip request:
        - Destination: {{command.Destination}}
        - Number of days: {{command.NumberOfDays}}
        - Budget: {{command.Budget}} {{command.Currency}}
        - Interests: {{string.Join(", ", command.Interests)}}

        Rules:
        - Return exactly {{command.NumberOfDays}} days.
        - Each day must include 2 to 4 activities.
        - Each day must include at least 1 restaurant suggestion.
        - Estimated costs must be numbers, not strings.
        - Keep the total estimated activity and restaurant costs reasonable for the budget.
        - Return only valid JSON.
        - Do not wrap the JSON in Markdown.
        - Do not include explanations outside the JSON.
        - Each activity must include durationHours as a number between 0.5 and 4.
        - transitMinutesFromPrevious must be 0 for the first activity of each day.
        - transitMinutesFromPrevious should be a realistic number between 0 and 90.

        Additional instruction:
        - {{additionalInstructionText}}


        JSON shape:
        {
            "overview": "string",
            "days": [
                {
                    "dayNumber": 1,
                    "title": "string",
                    "description": "string",
                    "activities": [
                        {
                            "timeOfDay": "Morning",
                            "title": "string",
                            "description": "string",
                            "estimatedCost": 50,
                            "durationHours": 2
                        }
                    ],
                    "restaurants": [
                        {
                            "name": "string",
                            "cuisine": "string",
                            "notes": "string",
                            "estimatedCost": 40
                        }
                    ]
                }
            ],
            "highlights": ["string"],
            "travelTips": ["string"]
        }
        """;
    }
}
