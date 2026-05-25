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

It uses deterministic placeholder logic instead of AI. This keeps the first version simple while preserving a clean place to introduce OpenAI later.
