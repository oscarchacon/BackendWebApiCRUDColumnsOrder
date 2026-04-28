using System.Reflection;
using Entities;
using Entities.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests.WebApi.Helpers;

public class InMemoryDatabaseInitializerTests
{
    [Fact]
    public void Initialize_WhenNoSeedConfigured_ShouldNotAddRows()
    {
        using var context = BuildContext();
        var configuration = new ConfigurationBuilder().Build();

        InvokeInitializer(context, configuration, Path.GetTempPath());

        context.Entities.Should().BeEmpty();
    }

    [Fact]
    public void Initialize_WhenSeedFileExists_ShouldPopulateEntities()
    {
        using var context = BuildContext();
        var tempDir = Path.Combine(Path.GetTempPath(), $"seed-test-{Guid.NewGuid()}");
        Directory.CreateDirectory(tempDir);
        var seedPath = Path.Combine(tempDir, "seed.json");
        File.WriteAllText(seedPath, "{" +
            "\"Entity\":[{" +
            "\"Id\":\"d2f8f277-a8b2-4fe8-ac66-1c9e35d97668\"," +
            "\"Name\":\"Seed Name\"," +
            "\"Description\":\"Seed Description\"," +
            "\"RegisterDate\":\"2026-04-01T10:00:00Z\"}]}");

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["SeedData:File"] = seedPath
            })
            .Build();

        InvokeInitializer(context, configuration, tempDir);

        context.Entities.Should().ContainSingle();
        var entity = context.Entities.Single();
        entity.Name.Should().Be("Seed Name");
    }

    private static RepositoryContext BuildContext()
    {
        var options = new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase($"initializer-tests-{Guid.NewGuid()}")
            .Options;

        return new RepositoryContext(options);
    }

    private static void InvokeInitializer(RepositoryContext context, IConfiguration configuration, string contentRootPath)
    {
        var type = typeof(global::WebApi.Controllers.EntityController).Assembly.GetType("WebApi.Helpers.InMemoryDatabaseInitializer", throwOnError: true)!;
        var method = type.GetMethod("Initialize", BindingFlags.NonPublic | BindingFlags.Static)!;
        method.Invoke(null, new object[] { context, configuration, contentRootPath });
    }
}
