using System.ComponentModel.DataAnnotations;

namespace AiTravelPlanner.Api.Contracts.Trips;

public sealed record GenerateTripRequest(
    [Required]
    [StringLength(120, MinimumLength = 2)]
    string Destination,

    [Range(1, 30)]
    int NumberOfDays,

    [Range(1, 1_000_000)]
    decimal Budget,

    [Required]
    [MinLength(1)]
    IReadOnlyCollection<string> Interests);