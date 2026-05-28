using System.ComponentModel.DataAnnotations;

namespace AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;

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
    [MaxLength(10)]
    IReadOnlyCollection<string> Interests,

    [Required]
    [StringLength(3, MinimumLength = 3)]
    string Currency);
