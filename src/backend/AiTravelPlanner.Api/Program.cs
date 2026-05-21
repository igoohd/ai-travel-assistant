using AiTravelPlanner.Application.Trips.GenerateTrip;
using AiTravelPlanner.Application.Trips.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<ITripPlanGenerator, StubTripPlanGenerator>();
builder.Services.AddScoped<IGenerateTripUseCase, GenerateTripHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
