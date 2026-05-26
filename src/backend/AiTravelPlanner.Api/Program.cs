using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Application.Trips.Services;
using AiTravelPlanner.Infrastructure.Ai;
using AiTravelPlanner.Infrastructure.Ai.GitHubModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var aiProviderOptions = builder.Configuration
    .GetSection(AiProviderOptions.SectionName)
    .Get<AiProviderOptions>() ?? new AiProviderOptions();

if (aiProviderOptions.ActiveProvider.Equals("GitHubModels", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<ITripPlanGenerator, GitHubModelsTripPlanGenerator>();
}
else
{
    builder.Services.AddScoped<ITripPlanGenerator, StubTripPlanGenerator>();
}

builder.Services.AddScoped<ITripPlanValidator, TripPlanValidator>();
builder.Services.AddScoped<IGenerateTripUseCase, GenerateTripHandler>();

builder.Services
    .AddOptions<GitHubModelsOptions>()
    .Bind(builder.Configuration.GetSection(GitHubModelsOptions.SectionName))
    .Validate(options => !string.IsNullOrWhiteSpace(options.Endpoint), "GitHub Models endpoint is required.")
    .Validate(options => !string.IsNullOrWhiteSpace(options.Model), "GitHub Models model is required.")
    .Validate(options => options.MaxTokens > 0, "GitHub Models max tokens must be greater than zero.")
    .Validate(options => options.Temperature >= 0 && options.Temperature <= 1, "GitHub Models temperature must be between 0 and 1.");
builder.Services.AddHttpClient<IGitHubModelsClient, GitHubModelsClient>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
