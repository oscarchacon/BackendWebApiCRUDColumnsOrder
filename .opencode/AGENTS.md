# AGENTS.md

## Agent profile for this repo
- Work as a .NET 5+ expert: ASP.NET Core, EF Core, layering, and dependency injection decisions should follow modern .NET practices.
- Treat current target frameworks as a baseline, not a permanent standard; keep changes upgrade-friendly for future framework updates.

## Scope and stack
- Solution entrypoint: `BackendWebApiCRUDColumnsOrder.sln`.
- Current project targets (verify before major changes; these are expected to evolve): `WebApi` (`net5.0`), `BusinesRules` (`netstandard2.1`), `Repository` (`netstandard2.1`), `Contracts` (`netstandard2.1`), `Entities` (`netstandard2.1`).
- Dependency flow is layered: `WebApi -> BusinesRules -> Repository -> Contracts -> Entities`.

## Verified developer commands
- Restore: `dotnet restore BackendWebApiCRUDColumnsOrder.sln`
- Build all: `dotnet build BackendWebApiCRUDColumnsOrder.sln`
- Run API: `dotnet run --project WebApi/WebApi.csproj`
- Run tests (when a test project exists): `dotnet test BackendWebApiCRUDColumnsOrder.sln`
- Run one test (when tests exist): `dotnet test --filter "FullyQualifiedName~Namespace.Class.Method"`

## Runtime quirks that affect changes
- `WebApi/Startup.cs` recreates DB on startup (`EnsureDeleted()` + `EnsureCreated()` in `Configure`), so local data is wiped each run.
- SQLite uses a relative path in `WebApi/Extensions/ServiceExtensions.cs`: `Data Source=./storage/DBstorage.db`.
- `DBstorage.db` is gitignored (`.gitignore`), so do not expect it to be committed.
- Swagger UI is served at root (`/`) because `RoutePrefix = string.Empty` in `WebApi/Extensions/AppExtensions.cs`.

## Code-change guidance for this repo
- Keep controllers thin (`WebApi/Controllers/*`), business logic in `BusinesRules`, and data access in `Repository`.
- If adding services/repositories/business rules, register DI in `WebApi/Extensions/ServiceExtensions.cs`.
- If changing `Entity` shape, verify ordering/paging and mapping paths:
  - `Repository/Entities/EntityRepository.cs`
  - `Repository/Utils/EFExtensions.cs`
  - `Entities/Helpers/AutoMapperProfile.cs`

## Testing expectations
- No test project is currently present (`*Test*.csproj` not found); create one when introducing non-trivial logic changes.
- Prioritize unit tests for `BusinesRules/Entities/EntitiesBR.cs` and repository utility behaviors (`Repository/Utils/*`).
- Do not claim tests passed unless `dotnet test` was executed.

## Known implementation pitfall
- `Repository/Entities/EntityRepository.cs` sets IDs with `new Guid()` (empty GUID) in `CreateEntity`; treat as a bug-prone area and cover with tests if touched.
