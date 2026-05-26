using AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;
using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Application.Trips.GetTrip;
using AiTravelPlanner.Application.Trips.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiTravelPlanner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TripsController : ControllerBase
{
    private readonly IGenerateTripUseCase _generateTripUseCase;
    private readonly IGetTripUseCase _getTripUseCase;
    public TripsController(IGenerateTripUseCase generateTripUseCase, IGetTripUseCase getTripUseCase)
    {
        _generateTripUseCase = generateTripUseCase;
        _getTripUseCase = getTripUseCase;
    }

    [HttpPost("generate")]
    [ProducesResponseType<GenerateTripResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenerateTripResponse>> GenerateTrip(
        [FromBody] GenerateTripRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await _generateTripUseCase.HandleAsync(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.ToResponse());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<GenerateTripResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenerateTripResponse>> GetTripById(Guid id, [FromServices] ITripPlanRepository tripPlanRepository, CancellationToken cancellationToken)
    {

        var result = await _getTripUseCase.HandleAsync(
                new GetTripQuery(id),
                cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result.Plan!.ToResponse());
    }
}
