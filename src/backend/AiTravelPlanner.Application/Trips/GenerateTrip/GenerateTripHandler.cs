using AiTravelPlanner.Application.Trips.Services;
using AiTravelPlanner.Domain.Trips;
using Microsoft.Extensions.Logging;

namespace AiTravelPlanner.Application.Trips.GenerateTrip;

public sealed class GenerateTripHandler : IGenerateTripUseCase
{
    private readonly ITripPlanGenerator _tripPlanGenerator;
    private readonly ITripPlanValidator _tripPlanValidator;
    private readonly ITripPlanRepository _tripPlanRepository;
    private readonly ITripInputSanitizer _tripInputSanitizer;
    private readonly ILogger<GenerateTripHandler> _logger;

    public GenerateTripHandler(
        ITripPlanGenerator tripPlanGenerator,
        ITripPlanValidator tripPlanValidator,
        ITripPlanRepository tripPlanRepository,
        ITripInputSanitizer tripInputSanitizer,
        ILogger<GenerateTripHandler> logger)
    {
        _tripPlanGenerator = tripPlanGenerator;
        _tripPlanValidator = tripPlanValidator;
        _tripPlanRepository = tripPlanRepository;
        _tripInputSanitizer = tripInputSanitizer;
        _logger = logger;
    }

    public async Task<GenerateTripResult> HandleAsync(GenerateTripCommand command, CancellationToken cancellationToken = default)
    {
        var startedAt = DateTimeOffset.UtcNow;
        var retryCount = 0;
        var retryReasons = new List<string>();
        var durationMs = 0;

        var sanitizedCommand = command with
        {
            Destination = _tripInputSanitizer.SanitizeInput(command.Destination),
            Currency = command.Currency.Trim().ToUpperInvariant(),
            Interests = command.Interests
                .Select(_tripInputSanitizer.SanitizeInput)
                .Where(interest => !string.IsNullOrWhiteSpace(interest))
                .ToArray()
        };

        if (sanitizedCommand.NumberOfDays <= 0)
        {
            _logger.LogWarning(
                "Generate trip rejected. Destination: {Destination}. Days: {NumberOfDays}. Reason: {Reason}.",
                sanitizedCommand.Destination,
                sanitizedCommand.NumberOfDays,
                "Number of days must be greater than zero.");

            return GenerateTripResult.Failure(
            [
                "Number of days must be greater than zero."
            ]);
        }

        if (sanitizedCommand.Interests.Count == 0)
        {
            _logger.LogWarning(
                "Generate trip rejected. Destination: {Destination}. Days: {NumberOfDays}. Reason: {Reason}.",
                sanitizedCommand.Destination,
                sanitizedCommand.NumberOfDays,
                "At least one interest must be provided.");

            return GenerateTripResult.Failure(
            [
                "At least one interest must be provided."
            ]);
        }

        if (sanitizedCommand.Interests.Any(interest => interest.Length > 50))
        {
            _logger.LogWarning(
                "Generate trip rejected. Destination: {Destination}. Days: {NumberOfDays}. Reason: {Reason}.",
                sanitizedCommand.Destination,
                sanitizedCommand.NumberOfDays,
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
                sanitizedCommand,
                cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Trip generation failed. Destination: {Destination}.",
                sanitizedCommand.Destination);

            return GenerateTripResult.Failure(
            [
                "Trip generation failed. Please try again."
            ]);
        }

        var validationIssues = _tripPlanValidator.Validate(plan, sanitizedCommand);

        if (validationIssues.Any(issue => issue.Code == ValidationIssueCodes.BudgetExceeded))
        {
            retryReasons.Add(ValidationIssueCodes.BudgetExceeded);
            _logger.LogInformation(
                "Trip generation retry triggered. Destination: {Destination}. Reason: {Reason}. EstimatedTotal: {EstimatedTotal}. RequestedBudget: {RequestedBudget}.",
                sanitizedCommand.Destination,
                ValidationIssueCodes.BudgetExceeded,
                plan.Budget.Total,
                sanitizedCommand.Budget);

            try
            {
                plan = await _tripPlanGenerator.GenerateAsync(
                    sanitizedCommand,
                    cancellationToken,
                    "The previous plan exceeded the budget. Generate a cheaper version with lower activity and restaurant costs.");
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Trip regeneration failed. Destination: {Destination}.",
                    sanitizedCommand.Destination);

                durationMs = (int)Math.Round((DateTimeOffset.UtcNow - startedAt).TotalMilliseconds);
                return GenerateTripResult.Success(plan, validationIssues, retryCount, retryReasons, durationMs);
            }

            retryCount++;
            validationIssues = _tripPlanValidator.Validate(plan, sanitizedCommand);
        }

        await _tripPlanRepository.SaveAsync(
            plan,
            sanitizedCommand,
            validationIssues,
            cancellationToken);

        durationMs = (int)Math.Round((DateTimeOffset.UtcNow - startedAt).TotalMilliseconds);
        _logger.LogInformation(
            "Generate trip completed. Destination: {Destination}. Days: {NumberOfDays}. Success: {Success}. RetryCount: {RetryCount}. DurationMs: {DurationMs}.",
            sanitizedCommand.Destination,
            sanitizedCommand.NumberOfDays,
            true,
            retryCount,
            durationMs);

        return GenerateTripResult.Success(plan, validationIssues, retryCount, retryReasons, durationMs);
    }
}
