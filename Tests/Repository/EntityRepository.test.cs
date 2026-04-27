using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Tests.Repository;

public class EntityRepositoryTests
{
    [Fact]
    public void GetAll_WhenSortingByNameDescending_ReturnsSortedRows()
    {
        using var context = BuildContext();
        Seed(context,
            new Entity { Id = Guid.NewGuid(), Name = "Alpha", Description = "A", RegisterDate = DateTime.UtcNow },
            new Entity { Id = Guid.NewGuid(), Name = "Gamma", Description = "G", RegisterDate = DateTime.UtcNow },
            new Entity { Id = Guid.NewGuid(), Name = "Beta", Description = "B", RegisterDate = DateTime.UtcNow });
        var repository = new EntityRepository(context);

        var result = repository.GetAll(columnName: "Name", orderDesc: true).ToList();

        Assert.Equal(new[] { "Gamma", "Beta", "Alpha" }, result.Select(entity => entity.Name).ToArray());
    }

    [Fact]
    public void GetAllPaged_WhenPageAndSizeProvided_ReturnsExpectedPage()
    {
        using var context = BuildContext();
        Seed(context,
            new Entity { Id = Guid.NewGuid(), Name = "A", Description = "A", RegisterDate = DateTime.UtcNow },
            new Entity { Id = Guid.NewGuid(), Name = "B", Description = "B", RegisterDate = DateTime.UtcNow },
            new Entity { Id = Guid.NewGuid(), Name = "C", Description = "C", RegisterDate = DateTime.UtcNow },
            new Entity { Id = Guid.NewGuid(), Name = "D", Description = "D", RegisterDate = DateTime.UtcNow });
        var repository = new EntityRepository(context);

        var result = repository.GetAllPaged(page: 2, pageSize: 2, columnName: "Name");

        Assert.Equal(4, result.RowCount);
        Assert.Equal(2, result.CurrentPage);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(new[] { "C", "D" }, result.Results.Select(entity => entity.Name).ToArray());
    }

    [Fact]
    public void GetById_WhenEntityDoesNotExist_ReturnsEmptyEntity()
    {
        using var context = BuildContext();
        var repository = new EntityRepository(context);

        var result = repository.GetById(Guid.NewGuid());

        Assert.NotNull(result);
        Assert.Equal(Guid.Empty, result.Id);
    }

    [Fact]
    public void UpdateEntity_WhenCalled_MapsNameAndDescription()
    {
        using var context = BuildContext();
        var id = Guid.NewGuid();
        var current = new Entity { Id = id, Name = "Current", Description = "Current description", RegisterDate = DateTime.UtcNow };
        Seed(context, current);
        context.ChangeTracker.Clear();
        var repository = new EntityRepository(context);

        var dbEntity = repository.GetById(id);
        var updated = new Entity { Name = "Updated", Description = "Updated description", RegisterDate = DateTime.UtcNow.AddDays(2) };
        repository.UpdateEntity(dbEntity, updated);
        context.SaveChanges();

        var persisted = context.Entities.Single(entity => entity.Id == id);
        Assert.Equal("Updated", persisted.Name);
        Assert.Equal("Updated description", persisted.Description);
    }

    private static RepositoryContext BuildContext()
    {
        var options = new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase($"repo-tests-{Guid.NewGuid()}")
            .Options;

        return new RepositoryContext(options);
    }

    private static void Seed(RepositoryContext context, params Entity[] entities)
    {
        context.Entities.AddRange(entities);
        context.SaveChanges();
    }
}
