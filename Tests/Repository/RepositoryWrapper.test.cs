using Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repository.Wrappers;

namespace Tests.Repository;

public class RepositoryWrapperTests
{
    [Fact]
    public void Save_WhenCalled_UsesRepositoryContextSaveChanges()
    {
        var options = new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase($"wrapper-save-{Guid.NewGuid()}")
            .Options;

        var contextMock = new Mock<RepositoryContext>(options);
        var wrapper = new RepositoryWrapper(contextMock.Object);

        wrapper.Save();

        contextMock.Verify(context => context.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Entity_WhenAccessedTwice_ReturnsSameRepositoryInstance()
    {
        var options = new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase($"wrapper-entity-{Guid.NewGuid()}")
            .Options;

        using var context = new RepositoryContext(options);
        var wrapper = new RepositoryWrapper(context);

        var first = wrapper.Entity;
        var second = wrapper.Entity;

        first.Should().NotBeNull();
        first.Should().BeSameAs(second);
    }
}
