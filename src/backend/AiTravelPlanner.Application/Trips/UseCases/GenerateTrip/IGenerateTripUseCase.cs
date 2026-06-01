namespace AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;

public interface IGenerateTripUseCase
{
    Task<GenerateTripResult> HandleAsync(GenerateTripCommand command, CancellationToken cancellationToken);
}
