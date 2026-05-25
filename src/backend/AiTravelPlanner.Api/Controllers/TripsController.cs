using AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;
using AiTravelPlanner.Application.Trips.GenerateTrip;
using Microsoft.AspNetCore.Mvc;

namespace AiTravelPlanner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TripsController : ControllerBase
{
    private readonly IGenerateTripUseCase _generateTripUseCase;

    public TripsController(IGenerateTripUseCase generateTripUseCase)
    {
        _generateTripUseCase = generateTripUseCase;
    }

    [HttpPost("generate")]
    public ActionResult<GenerateTripResponse> GenerateTrip([FromBody] GenerateTripRequest request)
    {
        var command = request.ToCommand();

        var tripPlan = _generateTripUseCase.Handle(command);

        if (!tripPlan.IsSuccess)
        {
            return BadRequest(tripPlan.Errors);
        }

        return Ok(tripPlan.Plan!.ToResponse());
    }
}
