# AI Travel Planner Backend

## Architecture

The backend follows a Clean Architecture-inspired structure.

```text
AiTravelPlanner.Api
  -> HTTP controllers and API contracts

AiTravelPlanner.Application
  -> use cases, ports, validation, sanitization, orchestration

AiTravelPlanner.Domain
  -> core trip planning models and domain rules

AiTravelPlanner.Infrastructure
  -> external integrations such as AI providers, databases, telemetry
```

## Application Structure

```text
Application/Trips/UseCases
  -> workflows/actions such as GenerateTrip, GetTrip, ListTrips, ValidateTrip

Application/Trips/Ports
  -> interfaces the application needs from outside layers

Application/Trips/Validation
  -> trip validation rules

Application/Trips/Sanitization
  -> input cleanup before use case execution
```

Infrastructure provides concrete implementations for application ports:

```text
Infrastructure/Ai/GitHubModels
  -> raw GitHub Models HTTP adapter

Infrastructure/Ai/ExtensionsAi
  -> Microsoft.Extensions.AI adapter using IChatClient

Infrastructure/Ai/Stub
  -> deterministic local fallback generator

Infrastructure/Persistence
  -> trip storage adapter
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
  -> GitHubModelsTripPlanGenerator, ExtensionsAiTripPlanGenerator, or StubTripPlanGenerator
  -> IGitHubModelsClient when raw GitHub Models is active
  -> IChatClient when Extensions.AI is active
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

Supported values:

```text
GitHubModels
ExtensionsAi
Stub
```

Set it to `GitHubModels` to use the raw GitHub Models HTTP integration.

Set it to `ExtensionsAi` to use Microsoft.Extensions.AI through `IChatClient` and an OpenAI-compatible adapter pointed at GitHub Models.

Any other value falls back to `StubTripPlanGenerator`.

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

## Extensions.AI Setup

Extensions.AI configuration lives under:

```text
AiProviders:ExtensionsAi
```

This provider uses `Microsoft.Extensions.AI.IChatClient` with the OpenAI-compatible adapter. GitHub Models still hosts the model, but the application talks to it through the .NET-native `IChatClient` abstraction.

The Extensions.AI token is also stored with user-secrets:

```bash
dotnet user-secrets set "AiProviders:ExtensionsAi:Token" "YOUR_GITHUB_TOKEN" --project src/backend/AiTravelPlanner.Api
```

When `AiProviders:ActiveProvider` is `ExtensionsAi`, the app calls GitHub Models through `ExtensionsAiTripPlanGenerator`.
