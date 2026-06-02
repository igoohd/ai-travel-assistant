using System.ClientModel;
using AiTravelPlanner.Application.Trips.Ports;
using AiTravelPlanner.Application.Trips.Prompting;
using AiTravelPlanner.Application.Trips.Sanitization;
using AiTravelPlanner.Application.Trips.UseCases.GenerateTrip;
using AiTravelPlanner.Application.Trips.UseCases.GetTrip;
using AiTravelPlanner.Application.Trips.UseCases.ListTrips;
using AiTravelPlanner.Application.Trips.UseCases.ValidateTrip;
using AiTravelPlanner.Application.Trips.Validation;
using AiTravelPlanner.Infrastructure.Ai;
using AiTravelPlanner.Infrastructure.Ai.ExtensionsAi;
using AiTravelPlanner.Infrastructure.Ai.GitHubModels;
using AiTravelPlanner.Infrastructure.Ai.Stub;
using AiTravelPlanner.Infrastructure.Persistence;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;

const string FrontendCorsPolicy = "FrontendCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(FrontendCorsPolicy, policy =>
        {
            policy
                .WithOrigins("http://localhost:3000", "http://localhost:3001")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });
}

var aiProviderOptions = builder.Configuration
    .GetSection(AiProviderOptions.SectionName)
    .Get<AiProviderOptions>() ?? new AiProviderOptions();

if (aiProviderOptions.ActiveProvider.Equals("GitHubModels", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<ITripPlanGenerator, GitHubModelsTripPlanGenerator>();
}
else if (aiProviderOptions.ActiveProvider.Equals("ExtensionsAi", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<ITripPlanGenerator, ExtensionsAiTripPlanGenerator>();
}
else
{
    builder.Services.AddScoped<ITripPlanGenerator, StubTripPlanGenerator>();
}

builder.Services.AddScoped<ITripPlanValidator, TripPlanValidator>();
builder.Services.AddScoped<IGenerateTripUseCase, GenerateTripHandler>();
builder.Services.AddScoped<IValidateTripUseCase, ValidateTripHandler>();
builder.Services.AddScoped<IGetTripUseCase, GetTripHandler>();
builder.Services.AddScoped<IListTripsUseCase, ListTripsHandler>();

builder.Services
    .AddOptions<GitHubModelsOptions>()
    .Bind(builder.Configuration.GetSection(GitHubModelsOptions.SectionName))
    .Validate(options => !string.IsNullOrWhiteSpace(options.Endpoint), "GitHub Models endpoint is required.")
    .Validate(options => !string.IsNullOrWhiteSpace(options.Model), "GitHub Models model is required.")
    .Validate(options => options.MaxTokens > 0, "GitHub Models max tokens must be greater than zero.")
    .Validate(options => options.Temperature >= 0 && options.Temperature <= 1, "GitHub Models temperature must be between 0 and 1.");
builder.Services.AddHttpClient<IGitHubModelsClient, GitHubModelsClient>();

builder.Services.AddSingleton<IChatClient>(serviceProvider =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<GitHubModelsOptions>>()
        .Value;

    var chatClient = new ChatClient(
        model: options.Model,
        credential: new ApiKeyCredential(options.Token),
        options: new OpenAIClientOptions
        {
            Endpoint = new Uri(options.Endpoint)
        });

    return chatClient.AsIChatClient();
});
builder.Services.AddSingleton<ITripPlanRepository, InMemoryTripPlanRepository>();
builder.Services.AddSingleton<ITripInputSanitizer, TripInputSanitizer>();
builder.Services.AddSingleton<ITripGenerationPromptBuilder, TripGenerationPromptBuilder>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseCors(FrontendCorsPolicy);
}

app.UseAuthorization();

app.MapControllers();

app.Run();
