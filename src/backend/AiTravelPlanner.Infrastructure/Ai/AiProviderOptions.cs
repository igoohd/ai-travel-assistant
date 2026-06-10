namespace AiTravelPlanner.Infrastructure.Ai;

public sealed class AiProviderOptions
{
    public const string SectionName = "AiProviders";

    public string ActiveProvider { get; init; } = AiProviderNames.Stub;
}
