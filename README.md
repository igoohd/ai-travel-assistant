# Source Index

This directory stores source material used to guide the AI Travel Planner Assistant project.

## AI Travel Planner PRD

Source file: `ai-travel-planner-prd.md`

Original file: `/Users/igor/Desktop/AI Travel Planner - Product Requirements Document (PRD) .rtf`

Imported on: 2026-05-20

### Product Intent

Build a small but technically rich AI travel planning application that evolves through four learning phases:

1. Raw LLM integration with the OpenAI SDK.
2. Reusable orchestration with LangChain.
3. Stateful workflow orchestration with LangGraph.
4. Multi-agent architecture with Microsoft Agent Framework.

### Core Product Scope

The assistant should generate personalized travel itineraries from destination, trip length, budget, and interests. It should estimate budgets, validate unrealistic or over-budget plans, regenerate when constraints fail, and produce a final trip summary.

### Initial Agent Model

The PRD defines five specialized agents for the later architecture:

* `ResearchAgent`: researches attractions and activities.
* `BudgetAgent`: estimates travel costs.
* `PlannerAgent`: generates itinerary structure.
* `ValidatorAgent`: validates conflicts and constraints.
* `SummaryAgent`: creates the final summarized plan.

### Key Non-Functional Requirements

* Average response time below 10 seconds.
* Optional streaming responses.
* Multi-provider AI support.
* Future extensibility for additional agents.
* Observability for token usage, latency, retries, workflow execution, and failures.
* Prompt sanitization, request size limits, and input validation.

### Implementation Implications

The PRD is intentionally learning-driven, so the implementation should preserve clear phase boundaries. Early code should stay simple enough to demonstrate raw SDK usage before introducing abstractions. Later phases should add orchestration, validation loops, state, and agent coordination as visible architectural increments rather than jumping directly to the final multi-agent form.
