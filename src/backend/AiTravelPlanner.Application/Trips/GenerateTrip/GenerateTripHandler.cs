using AiTravelPlanner.Application.Trips.Services;
using AiTravelPlanner.Domain.Trips;
using Microsoft.Extensions.Logging;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public sealed class GenerateTripHandler : IGenerateTripUseCase
{
    private readonly ITripPlanGenerator _tripPlanGenerator;
    private readonly ITripPlanValidator _tripPlanValidator;
    private readonly ILogger<GenerateTripHandler> _logger;

    public GenerateTripHandler(
        ITripPlanGenerator tripPlanGenerator,
        ITripPlanValidator tripPlanValidator,
        ILogger<GenerateTripHandler> logger)
    {
        _tripPlanGenerator = tripPlanGenerator;
        _tripPlanValidator = tripPlanValidator;
        _logger = logger;
    }

    public async Task<GenerateTripResult> HandleAsync(GenerateTripCommand command)
    {
        var startedAt = DateTimeOffset.UtcNow;
        var retryCount = 0;

        if (command.NumberOfDays <= 0)
        {
            _logger.LogWarning(
                "Generate trip rejected. Destination: {Destination}. Days: {NumberOfDays}. Reason: {Reason}.",
                command.Destination,
                command.NumberOfDays,
                "Number of days must be greater than zero.");

            return GenerateTripResult.Failure(
            [
                "Number of days must be greater than zero."
            ]);
        }

        var plan = await _tripPlanGenerator.GenerateAsync(command);
        var validationIssues = _tripPlanValidator.Validate(plan, command);

        if (validationIssues.Any(issue => issue.Code == ValidationIssueCodes.BudgetExceeded))
        {
            plan = await _tripPlanGenerator.GenerateAsync(
                command,
                "The previous plan exceeded the budget. Generate a cheaper version with lower activity and restaurant costs.");

            retryCount++;
            validationIssues = _tripPlanValidator.Validate(plan, command);
        }

        _logger.LogInformation(
            "Generate trip completed. Destination: {Destination}. Days: {NumberOfDays}. Success: {Success}. RetryCount: {RetryCount}. DurationMs: {DurationMs}.",
            command.Destination,
            command.NumberOfDays,
            true,
            retryCount,
            (DateTimeOffset.UtcNow - startedAt).TotalMilliseconds);

        return GenerateTripResult.Success(plan, validationIssues);
    }
}
