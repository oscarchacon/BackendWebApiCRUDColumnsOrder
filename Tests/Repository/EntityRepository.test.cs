using Entities;
using Entities.Models;
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

        Assert.Equal(new[] { "Gamma", "Beta", "Alpha" }, result.Select(x => x.Name).ToArray());
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

        Assert.Equal(4, result.RowCount);
        Assert.Equal(2, result.CurrentPage);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(new[] { "C", "D" }, result.Results.Select(x => x.Name).ToArray());
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ReturnsEmptyEntity()
    {
        using var context = BuildContext();
        var repository = new EntityRepository(context);

        var result = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.NotNull(result);
        Assert.Equal(Guid.Empty, result.Id);
    }

    [Fact]
    public void GetAll_WhenCalled_ThrowsNotImplementedException()
    {
        using var context = BuildContext();
        var repository = new EntityRepository(context);

        Assert.Throws<NotImplementedException>(() => repository.GetAll());
    }

    [Fact]
    public void GetAllPaged_WhenCalled_ThrowsNotImplementedException()
    {
        using var context = BuildContext();
        var repository = new EntityRepository(context);

        Assert.Throws<NotImplementedException>(() => repository.GetAllPaged());
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
        Assert.NotEqual(Guid.Empty, saved.Id);
        Assert.Equal("Created", saved.Name);
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
