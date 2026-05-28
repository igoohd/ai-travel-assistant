namespace AiTravelPlanner.Application.Trips.Services;

public interface ITripInputSanitizer
{
    string SanitizeInput(string input);
}