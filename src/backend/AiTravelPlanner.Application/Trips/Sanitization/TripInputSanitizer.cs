using System.Text.RegularExpressions;

namespace AiTravelPlanner.Application.Trips.Sanitization;

public sealed partial class TripInputSanitizer : ITripInputSanitizer
{
    public string SanitizeInput(string input)
    {
        var trimmedInput = input.Trim();

        return WhitespaceRegex()
            .Replace(trimmedInput, " ");
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();
}
