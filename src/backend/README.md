# AI Travel Planner Backend

## Architecture

The backend follows a Clean Architecture-inspired structure.

```text
AiTravelPlanner.Api
  -> HTTP controllers and API contracts

AiTravelPlanner.Application
  -> use cases, application services, orchestration

AiTravelPlanner.Domain
  -> core trip planning models and domain rules

AiTravelPlanner.Infrastructure
  -> external integrations such as AI providers, databases, telemetry
```

## Dependency Direction

```text
Api -> Application -> Domain
Api -> Infrastructure -> Application
```

The Domain project should not depend on ASP.NET Core, AI providers, databases, or infrastructure concerns.

## Current Flow

```text
POST /api/trips/generate
  -> GenerateTripRequest
  -> GenerateTripCommand
  -> GenerateTripHandler
  -> ITripPlanGenerator
  -> GitHubModelsTripPlanGenerator or StubTripPlanGenerator
  -> IGitHubModelsClient when GitHub Models is active
  -> ITripPlanValidator
  -> ITripPlanRepository
  -> Plan
  -> GenerateTripResponse
```

## Current Implementation

The current trip generator is selected by configuration:

```text
AiProviders:ActiveProvider
```

Set it to `GitHubModels` to use `GitHubModelsTripPlanGenerator`, or any other value to fall back to `StubTripPlanGenerator`.

`StubTripPlanGenerator` uses deterministic placeholder logic. This keeps local development simple while preserving a clean fallback when a real AI provider is not configured.

## GitHub Models Setup

GitHub Models configuration lives under:

```text
AiProviders:GitHubModels
```

Non-secret development settings are stored in `AiTravelPlanner.Api/appsettings.Development.json`.

The GitHub token must not be committed. Store it with .NET user-secrets:

```bash
dotnet user-secrets set "AiProviders:GitHubModels:Token" "YOUR_GITHUB_TOKEN" --project src/backend/AiTravelPlanner.Api
```

The token needs permission to use GitHub Models. For a fine-grained personal access token, GitHub's REST API docs describe the required scope as `models: read`.

When `AiProviders:ActiveProvider` is `GitHubModels`, the app calls GitHub Models through `GitHubModelsTripPlanGenerator`.
