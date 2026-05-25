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
    [ProducesResponseType<GenerateTripResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenerateTripResponse>> GenerateTrip([FromBody] GenerateTripRequest request)
    {
        var command = request.ToCommand();

        var result = await _generateTripUseCase.HandleAsync(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.ToResponse());
    }
}
