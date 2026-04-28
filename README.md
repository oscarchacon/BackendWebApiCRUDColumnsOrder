# BackendWebApiCRUDColumnsOrder

This repository is a Web API template that provides a complete CRUD flow for an entity model, including pagination and column ordering support. The solution now targets **.NET 8.0** across all projects.

## Current platform and package versions

- .NET target framework: **net8.0**
- All projects use **.NET 8.0**
- Language: **C#**
- Data framework: **Entity Framework Core 8.0.0**
- JSON serialization: **Newtonsoft.Json 13.0.3**
- Mapping: **AutoMapper 16.1.1**
- API documentation: **Swashbuckle.AspNetCore 6.5.0**
- API versioning: **Microsoft.AspNetCore.Mvc.Versioning 5.1.0**

## Project dependency summary

### Entities
- `AutoMapper` 16.1.1
- `Microsoft.EntityFrameworkCore` 8.0.0
- `Microsoft.EntityFrameworkCore.Tools` 8.0.0

### WebApi
- `Microsoft.AspNetCore.Mvc.NewtonsoftJson` 8.0.0
- `Microsoft.AspNetCore.Mvc.Versioning` 5.1.0
- `Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer` 5.1.0
- `Microsoft.EntityFrameworkCore.InMemory` 8.0.0
- `Microsoft.EntityFrameworkCore.Tools` 8.0.0
- `Newtonsoft.Json` 13.0.3
- `Swashbuckle.AspNetCore` 6.5.0
- `Swashbuckle.AspNetCore.Annotations` 6.5.0

### Tests
- `coverlet.collector` 6.0.4
- `Microsoft.EntityFrameworkCore.InMemory` 8.0.0
- `Microsoft.NET.Test.Sdk` 17.14.1
- `Moq` 4.20.70
- `xunit` 2.9.3
- `xunit.runner.visualstudio` 3.1.4
- `FluentAssertions` 8.0.0

### Contracts
- No direct NuGet package dependencies declared

### BusinessRules
- No direct NuGet package dependencies declared

### Repository
- No direct NuGet package dependencies declared

## Technologies used

- ASP.NET Core Web API
- Entity Framework Core 8.0
- AutoMapper
- Newtonsoft.Json
- Swagger / OpenAPI documentation
- xUnit test project with coverlet and Moq

## Requirements

- Install **.NET 8.0 SDK**
- Use an IDE or editor that supports .NET 8, such as Visual Studio 2022, Visual Studio Code, or Rider
- Restore NuGet packages before building the solution

## Repository structure

- `WebApi`: main API project with controllers, middleware, extension methods, and Swagger integration
- `BusinessRules`: business logic layer
- `Entities`: domain entities and EF Core model definitions
- `Contracts`: repository and service interfaces
- `Repository`: persistence layer and repository implementations
- `Tests`: unit and integration test project

## Notes

- The solution target framework has been updated from .NET 5 to **.NET 8.0**.
- Package versions are documented above so the current dependency matrix is registered in this README.

## License

This template is released under the MIT license.
