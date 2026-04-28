using Contracts.Entities;
using Entities.Models;
using Entities.Utils.Paged.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.Contracts;

public class IEntityRepositoryTests
{
    public static IEnumerable<object[]> ExpectedDomainMethods()
    {
        yield return new object[] { "GetAll", typeof(IEnumerable<Entity>) };
        yield return new object[] { "GetAllAsync", typeof(Task<IEnumerable<Entity>>) };
        yield return new object[] { "GetAllPaged", typeof(IPagedResult<Entity>) };
        yield return new object[] { "GetAllPagedAsync", typeof(Task<IPagedResult<Entity>>) };
        yield return new object[] { "GetById", typeof(Entity) };
        yield return new object[] { "GetByIdAsync", typeof(Task<Entity>) };
        yield return new object[] { "CreateEntity", typeof(void) };
        yield return new object[] { "CreateEntityAsync", typeof(Task) };
        yield return new object[] { "UpdateEntity", typeof(void) };
        yield return new object[] { "DeleteEntity", typeof(void) };
    }

    [Fact]
    public void IEntityRepository_ShouldInheritIRepositoryBaseOfEntity()
    {
        var inherited = typeof(IEntityRepository).GetInterfaces();

        inherited.Should().Contain(type =>
            type.IsGenericType
            && type.GetGenericTypeDefinition() == typeof(global::Contracts.Interfaces.IRepositoryBase<>)
            && type.GenericTypeArguments[0] == typeof(Entity));
    }

    [Theory]
    [MemberData(nameof(ExpectedDomainMethods))]
    public void IEntityRepository_ShouldExposeExpectedDomainMethods(string methodName, Type expectedReturnType)
    {
        var methods = typeof(IEntityRepository).GetMethods().ToDictionary(method => method.Name, method => method);

        methods[methodName].ReturnType.Should().Be(expectedReturnType);
    }

    [Fact]
    public async Task IEntityRepository_MoqSetup_ShouldSupportAsyncAndPagedContracts()
    {
        var mock = new Mock<IEntityRepository>();
        var id = Guid.NewGuid();
        var entity = new Entity { Id = id, Name = "Name", Description = "Desc", RegisterDate = DateTime.UtcNow };
        var paged = new global::Entities.Utils.Paged.PagedResult<Entity>
        {
            CurrentPage = 1,
            PageSize = 10,
            RowCount = 1,
            PageCount = 1,
            Results = new List<Entity> { entity }
        };

        mock.Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);
        mock.Setup(repository => repository.GetAllPagedAsync(null, null, null, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        var byId = await mock.Object.GetByIdAsync(id);
        var pagedResult = await mock.Object.GetAllPagedAsync();

        byId.Id.Should().Be(id);
        pagedResult.Results.Should().HaveCount(1);
    }

    [Fact]
    public async Task IEntityRepository_MoqSetup_WhenEntityDoesNotExist_ShouldReturnEmptyEntity()
    {
        var mock = new Mock<IEntityRepository>();
        mock.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Entity());

        var result = await mock.Object.GetByIdAsync(Guid.NewGuid());

        result.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public async Task IEntityRepository_MoqSetup_WhenGetAllPagedAsyncFails_ShouldPropagateException()
    {
        var mock = new Mock<IEntityRepository>();
        mock.Setup(repository => repository.GetAllPagedAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TimeoutException("Database timeout"));

        Func<Task> action = async () => await mock.Object.GetAllPagedAsync();

        await action.Should().ThrowAsync<TimeoutException>().WithMessage("Database timeout");
    }

    [Fact]
    public async Task IEntityRepository_MoqSetup_WhenCancellationTokenIsCancelled_ShouldThrowOperationCanceledException()
    {
        var mock = new Mock<IEntityRepository>();
        mock.Setup(repository => repository.GetAllAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .Returns<int?, int?, string, bool, CancellationToken>((_, _, _, _, token) =>
            {
                token.ThrowIfCancellationRequested();
                return Task.FromResult<IEnumerable<Entity>>(Array.Empty<Entity>());
            });

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        Func<Task> action = async () => await mock.Object.GetAllAsync(cancellationToken: cts.Token);

        await action.Should().ThrowAsync<OperationCanceledException>();
    }
}
