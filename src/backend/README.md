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

The Domain project should not depend on ASP.NET Core, OpenAI, databases, or infrastructure concerns.

## Current Flow

```text
POST /api/trips/generate
  -> GenerateTripRequest
  -> GenerateTripCommand
  -> GenerateTripHandler
  -> ITripPlanGenerator
  -> StubTripPlanGenerator
  -> ITripPlanValidator
  -> Plan
  -> GenerateTripResponse
```

## Current Implementation

The current trip generator is `StubTripPlanGenerator`.

It uses deterministic placeholder logic instead of AI. This keeps the first version simple while preserving a clean place to introduce a real AI provider later.

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

The current app still uses `StubTripPlanGenerator`; GitHub Models is configured but not called yet.
