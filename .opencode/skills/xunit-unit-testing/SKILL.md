# xUnit Unit Testing Skill (.NET 5+)

## Purpose
Use this skill when implementing or updating unit tests in C# projects that target .NET 5 or newer.

## Operating Rules
- Focus on unit tests first (fast, isolated, deterministic).
- Add or update tests in the same change as business logic changes.
- Prefer behavior-based test names: `Method_Scenario_ExpectedResult`.
- Keep one behavioral assertion per test intent; split when intent diverges.
- Never claim tests pass unless `dotnet test` was executed.

## Recommended Workflow
1. Identify the public behavior changed (not just internal implementation).
2. Add failing tests that describe expected behavior.
3. Implement/fix code with minimal change to pass tests.
4. Run focused tests first, then full test project/solution.
5. Refactor test code for readability while preserving behavior coverage.

## xUnit Patterns
- Use `[Fact]` for single scenario tests.
- Use `[Theory]` + `[InlineData]` or `[MemberData]` for input matrices.
- Use `Assert.Throws<T>()` / `Assert.ThrowsAsync<T>()` for exception behavior.
- Use `IClassFixture<T>` only when setup cost is high and shared state is safe.
- Avoid test interdependence; each test must run independently.

## Isolation and Dependencies
- Mock external dependencies (DB, HTTP, filesystem, clock, random, environment).
- Keep domain/business tests independent from EF Core provider behavior when possible.
- If repository behavior must be tested, prefer dedicated tests per repository concern.
- Avoid real network calls in unit tests.

## Practical Commands
- Run all tests in solution:
  - `dotnet test BackendWebApiCRUDColumnsOrder.sln`
- Run a specific test class/method:
  - `dotnet test --filter "FullyQualifiedName~Namespace.ClassName"`
  - `dotnet test --filter "FullyQualifiedName~Namespace.ClassName.MethodName"`

## Coverage Priorities (this repository)
- Business rules in `BusinesRules/Entities/EntitiesBR.cs`.
- Repository utility behavior in `Repository/Utils/*` (paging, ordering helpers).
- Controller validation/error branches in `WebApi/Controllers/EntityController.cs`.

## Quality Bar
- Every non-trivial code change should include:
  - at least one happy-path test,
  - at least one boundary or error-path test.
- Prefer explicit Arrange-Act-Assert structure for readability.
- Keep tests simple enough that failures explain what broke without debugging.
