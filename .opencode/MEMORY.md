# Persistent Memory (Cross-Session)

Purpose: reduce "new-session amnesia" by keeping stable context and user preferences in one file.

## How to use this file
- Read this file at the start of every session before proposing changes.
- After finishing a meaningful task, update only the sections that changed.
- Keep entries short, factual, and verifiable (no speculation).
- Prefer durable context (preferences, decisions, constraints) over temporary logs.

## User Preferences
- Preferred language: Spanish for conversation.
- Expectation: agent should behave like a .NET 5+ expert.
- Testing focus: give strong relevance to unit testing (xUnit preferred when adding tests).
- Framework targets are not final standards; changes should stay upgrade-friendly.

## Repository Facts (stable)
- Layered flow: `WebApi -> BusinesRules -> Repository -> Contracts -> Entities`.
- API startup currently recreates local DB each run in `WebApi/Startup.cs` (`EnsureDeleted` + `EnsureCreated`).
- SQLite path is relative: `./storage/DBstorage.db`.

## Session Memory Log (append newest first)
- 2026-04-26: Created/updated `AGENTS.md` with repo-specific instructions and upgrade-friendly .NET guidance.
- 2026-04-26: Added xUnit testing skill in English at `skills/xunit-unit-testing.md`.
- 2026-04-26: Clarified that current target frameworks are temporary and expected to evolve.

## Update Rule for Future Agents
- When the user gives a durable preference or decision, add it here immediately.
- When a preference is replaced, update the old line instead of duplicating conflicting rules.
