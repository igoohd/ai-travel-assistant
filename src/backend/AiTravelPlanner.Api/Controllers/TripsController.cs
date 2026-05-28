using AiTravelPlanner.Api.Contracts;
using AiTravelPlanner.Api.Contracts.Trips.GenerateTrip;
using AiTravelPlanner.Api.Contracts.Trips.ListTrips;
using AiTravelPlanner.Api.Contracts.Trips.ValidateTrip;
using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Application.Trips.GetTrip;
using AiTravelPlanner.Application.Trips.ListTrips;
using AiTravelPlanner.Application.Trips.ValidateTrip;
using Microsoft.AspNetCore.Mvc;

namespace AiTravelPlanner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TripsController : ControllerBase
{
    private readonly IGenerateTripUseCase _generateTripUseCase;
    private readonly IGetTripUseCase _getTripUseCase;
    private readonly IListTripsUseCase _listTripsUseCase;
    private readonly IValidateTripUseCase _validateTripUseCase;

    public TripsController(
        IGenerateTripUseCase generateTripUseCase,
        IGetTripUseCase getTripUseCase,
        IListTripsUseCase listTripsUseCase,
        IValidateTripUseCase validateTripUseCase)
    {
        _generateTripUseCase = generateTripUseCase;
        _getTripUseCase = getTripUseCase;
        _listTripsUseCase = listTripsUseCase;
        _validateTripUseCase = validateTripUseCase;
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
            return BadRequest(new ApiErrorResponse(result.Errors));
        }

        return Ok(result.ToResponse());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<GenerateTripResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenerateTripResponse>> GetTripById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getTripUseCase.HandleAsync(
            new GetTripQuery(id),
            cancellationToken);

        if (!result.IsFound)
        {
            return NotFound(new ApiErrorResponse(
            [
                "Trip not found."
            ]));
        }

        return Ok(result.Plan!.ToResponse());
    }

    [HttpGet]
    [ProducesResponseType<ListTripsResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ListTripsResponse>> List(CancellationToken cancellationToken)
    {
        var result = await _listTripsUseCase.HandleAsync(new ListTripsQuery(), cancellationToken);

        return Ok(result.ToResponse());
    }

    [HttpPost("{id:guid}/validate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ValidateTrip(Guid id, CancellationToken cancellationToken)
    {
        var result = await _validateTripUseCase.HandleAsync(new ValidateTripCommand(id), cancellationToken);

        if (!result.IsSuccessful)
        {
            return NotFound(new ApiErrorResponse(result.Errors));
        }

        return Ok(result.ToResponse());
    }
}
