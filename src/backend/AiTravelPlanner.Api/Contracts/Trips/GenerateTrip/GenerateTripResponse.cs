namespace AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;

public sealed record GenerateTripResponse(
    string Destination,
    int NumberOfDays,
    string Overview,
    IReadOnlyCollection<TripDayResponse> Days
    );

public sealed record TripDayResponse(
int DayNumber,
string Title,
string Description,
IReadOnlyCollection<TripActivityResponse> Activities);

public sealed record TripActivityResponse(
    string TimeOfDay,
    string Title,
    string Description
);
