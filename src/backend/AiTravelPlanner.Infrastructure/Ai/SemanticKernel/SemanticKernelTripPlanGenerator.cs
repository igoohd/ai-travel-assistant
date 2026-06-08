using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Domain.Trips;
using Microsoft.SemanticKernel;

namespace AiTravelPlanner.Infrastructure.Ai.SemanticKernel;

public sealed class SemanticKernelTripPlanGenerator : ITripPlanGenerator
{
    private readonly Kernel _kernel;

    public SemanticKernelTripPlanGenerator(Kernel kernel)
    {
        _kernel = kernel;
    }

    public Task<Plan> GenerateAsync(GenerateTripCommand command, CancellationToken cancellationToken, string? additionalInstruction = null)
    {
        throw new NotImplementedException("Semantic Kernel integration is not implemented yet");
    }
}