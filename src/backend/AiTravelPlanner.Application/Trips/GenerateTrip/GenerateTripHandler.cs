using AiTravelPlanner.Application.Trips.Services;
using AiTravelPlanner.Domain.Trips;
using Microsoft.Extensions.Logging;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public sealed class GenerateTripHandler : IGenerateTripUseCase
{
    private readonly ITripPlanGenerator _tripPlanGenerator;
    private readonly ITripPlanValidator _tripPlanValidator;
    private readonly ILogger<GenerateTripHandler> _logger;
    private readonly ITripPlanRepository _tripPlanRepository;

    public GenerateTripHandler(
        ITripPlanGenerator tripPlanGenerator,
        ITripPlanValidator tripPlanValidator,
        ITripPlanRepository tripPlanRepository,
        ILogger<GenerateTripHandler> logger)
    {
        _tripPlanGenerator = tripPlanGenerator;
        _tripPlanValidator = tripPlanValidator;
        _tripPlanRepository = tripPlanRepository;
        _logger = logger;
    }

    public async Task<GenerateTripResult> HandleAsync(GenerateTripCommand command, CancellationToken cancellationToken = default)
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

        if (command.Interests.Count == 0)
        {
            _logger.LogWarning(
                "Generate trip rejected. Destination: {Destination}. Days: {NumberOfDays}. Reason: {Reason}.",
                command.Destination,
                command.NumberOfDays,
                "At least one interest must be provided.");

            return GenerateTripResult.Failure(
            [
                "At least one interest must be provided."
            ]);
        }

        if (command.Interests.Any(interest => interest.Length > 50))
        {
            _logger.LogWarning(
                "Generate trip rejected. Destination: {Destination}. Days: {NumberOfDays}. Reason: {Reason}.",
                command.Destination,
                command.NumberOfDays,
                "Each interest must be 50 characters or fewer.");

            return GenerateTripResult.Failure(
            [
                "Each interest must be 50 characters or fewer."
            ]);
        }

        Plan plan;

        try
        {
            plan = await _tripPlanGenerator.GenerateAsync(
                command,
                cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Trip generation failed. Destination: {Destination}.",
                command.Destination);

            return GenerateTripResult.Failure(
            [
                "Trip generation failed. Please try again."
            ]);
        }


        var validationIssues = _tripPlanValidator.Validate(plan);

        if (validationIssues.Any(issue => issue.Code == ValidationIssueCodes.BudgetExceeded))
        {
            try
            {

                plan = await _tripPlanGenerator.GenerateAsync(
                    command,
                    cancellationToken,
                    "The previous plan exceeded the budget. Generate a cheaper version with lower activity and restaurant costs.");
            }
            catch (Exception exception)
            {
                _logger.LogError(
                exception,
                "Trip regeneration failed. Destination: {Destination}.",
                command.Destination);

                return GenerateTripResult.Success(plan, validationIssues, retryCount);
            }

            retryCount++;
            validationIssues = _tripPlanValidator.Validate(plan);
        }

        await _tripPlanRepository.SaveAsync(
            plan,
            command,
            validationIssues,
            cancellationToken);

        _logger.LogInformation(
            "Generate trip completed. Destination: {Destination}. Days: {NumberOfDays}. Success: {Success}. RetryCount: {RetryCount}. DurationMs: {DurationMs}.",
            command.Destination,
            command.NumberOfDays,
            true,
            retryCount,
            (DateTimeOffset.UtcNow - startedAt).TotalMilliseconds);

        return GenerateTripResult.Success(plan, validationIssues, retryCount);
    }
}
