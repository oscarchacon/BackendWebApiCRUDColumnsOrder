# Skill: .NET Web API Repo Assistant

## Purpose
Provide repository-specific guidance, code changes, and implementation support for this ASP.NET Core / EF Core CRUD API solution.

## Responsibilities
- Diagnose and implement requested feature work in the layered architecture: `WebApi -> BusinesRules -> Repository -> Contracts -> Entities`.
- Keep controllers thin and place business logic in `BusinesRules` and data access in `Repository`.
- Maintain compatibility with current target frameworks while keeping changes upgrade-friendly.
- Verify ordering, paging, mapping, and DI registration when entity shape or behavior changes.

## Repo-specific behavior
- Use `BackendWebApiCRUDColumnsOrder.sln` as the solution entrypoint.
- The current runtime stack uses `WebApi` targeting `net5.0`, and class libraries targeting `netstandard2.1`.
- `WebApi/Startup.cs` currently recreates the SQLite database on startup; do not assume persisted state across runs.
- Swagger is served at root (`/`) from `WebApi/Extensions/AppExtensions.cs`.
- SQLite path is configured relative to the app root in `WebApi/Extensions/ServiceExtensions.cs`.
- `Repository/Entities/EntityRepository.cs` currently contains an unsafe `new Guid()` usage in `CreateEntity` and should be treated as a likely bug-prone area.

## Implementation guidance
- When adding services, register them in `WebApi/Extensions/ServiceExtensions.cs`.
- When modifying entity models, inspect `Entities/Helpers/AutoMapperProfile.cs` and repository utility classes under `Repository/Utils`.
- Prefer adding tests for business rules and repository utilities if behavior changes.

## Common commands
- `dotnet restore BackendWebApiCRUDColumnsOrder.sln`
- `dotnet build BackendWebApiCRUDColumnsOrder.sln`
- `dotnet run --project WebApi/WebApi.csproj`
- `dotnet test BackendWebApiCRUDColumnsOrder.sln` (when tests exist)

## Notes
This skill file is meant to complement the existing `.opencode/AGENTS.md` profile and provide a local skill definition for repository-focused assistance.