using AiTravelPlanner.Api.Contracts.Trips;
using AiTravelPlanner.Application.Trips.GenerateTrip;
using Microsoft.AspNetCore.Mvc;

namespace AiTravelPlanner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TripsController : ControllerBase
{

    private readonly GenerateTripHandler _generateTripHandler;

    public TripsController(GenerateTripHandler generateTripHandler)
    {
        _generateTripHandler = generateTripHandler;
    }

    [HttpPost("generate")]
    public ActionResult GenerateTrip([FromBody] GenerateTripRequest request)
    {
        var command = new GenerateTripCommand(
            Destination: request.Destination,
            NumberOfDays: request.NumberOfDays,
            Budget: request.Budget,
            Interests: request.Interests
        );
        var tripPlan = _generateTripHandler.Handle(command);

        return Ok(tripPlan);
    }
}