using Entities;
using Entities.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Tests.Repository;

public class EntityRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_WhenOrderedByNameDescending_ReturnsSortedData()
    {
        using var context = BuildContext();
        Seed(context,
            new Entity { Id = Guid.NewGuid(), Name = "Alpha", Description = "A", RegisterDate = DateTime.UtcNow },
            new Entity { Id = Guid.NewGuid(), Name = "Gamma", Description = "G", RegisterDate = DateTime.UtcNow },
            new Entity { Id = Guid.NewGuid(), Name = "Beta", Description = "B", RegisterDate = DateTime.UtcNow });

        var repository = new EntityRepository(context);

        var result = (await repository.GetAllAsync(columnName: "Name", orderDesc: true)).ToList();

        result.Select(x => x.Name).Should().Equal(new[] { "Gamma", "Beta", "Alpha" });
    }

    [Fact]
    public async Task GetAllPagedAsync_WhenPageRequested_ReturnsExpectedPageData()
    {
        using var context = BuildContext();
        Seed(context,
            new Entity { Id = Guid.NewGuid(), Name = "A", Description = "A", RegisterDate = DateTime.UtcNow },
            new Entity { Id = Guid.NewGuid(), Name = "B", Description = "B", RegisterDate = DateTime.UtcNow },
            new Entity { Id = Guid.NewGuid(), Name = "C", Description = "C", RegisterDate = DateTime.UtcNow },
            new Entity { Id = Guid.NewGuid(), Name = "D", Description = "D", RegisterDate = DateTime.UtcNow });

        var repository = new EntityRepository(context);

        var result = await repository.GetAllPagedAsync(page: 2, pageSize: 2, columnName: "Name");

        result.RowCount.Should().Be(4);
        result.CurrentPage.Should().Be(2);
        result.PageSize.Should().Be(2);
        result.Results.Select(x => x.Name).Should().Equal(new[] { "C", "D" });
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ReturnsEmptyEntity()
    {
        using var context = BuildContext();
        var repository = new EntityRepository(context);

        var result = await repository.GetByIdAsync(Guid.NewGuid());

        result.Should().NotBeNull();
        result.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public void GetAll_WhenCalled_ThrowsNotImplementedException()
    {
        using var context = BuildContext();
        var repository = new EntityRepository(context);

        Action act = () => repository.GetAll();

        act.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void GetAllPaged_WhenCalled_ThrowsNotImplementedException()
    {
        using var context = BuildContext();
        var repository = new EntityRepository(context);

        Action act = () => repository.GetAllPaged();

        act.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task CreateEntityAsync_WhenCalled_AddsEntityWithGeneratedId()
    {
        using var context = BuildContext();
        var repository = new EntityRepository(context);
        var entity = new Entity { Name = "Created", Description = "Created row", RegisterDate = DateTime.UtcNow };

        await repository.CreateEntityAsync(entity);
        await context.SaveChangesAsync();

        var saved = await context.Entities.SingleAsync();
        saved.Id.Should().NotBe(Guid.Empty);
        saved.Name.Should().Be("Created");
    }

    private static RepositoryContext BuildContext()
    {
        var options = new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase($"entity-repository-tests-{Guid.NewGuid()}")
            .Options;

        return new RepositoryContext(options);
    }

    private static void Seed(RepositoryContext context, params Entity[] entities)
    {
        context.Entities.AddRange(entities);
        context.SaveChanges();
    }
}
