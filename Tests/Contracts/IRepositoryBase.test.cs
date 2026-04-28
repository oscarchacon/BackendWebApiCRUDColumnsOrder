using System.Linq.Expressions;
using Contracts.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.Contracts;

public class IRepositoryBaseTests
{
    public static IEnumerable<object[]> ExpectedMethodNames()
    {
        yield return new object[] { "FindAll" };
        yield return new object[] { "FindByCondition" };
        yield return new object[] { "Create" };
        yield return new object[] { "Update" };
        yield return new object[] { "Delete" };
        yield return new object[] { "SaveAsync" };
    }

    [Theory]
    [MemberData(nameof(ExpectedMethodNames))]
    public void IRepositoryBase_ShouldContainExpectedMethod(string methodName)
    {
        var type = typeof(IRepositoryBase<>);
        var methods = type.GetMethods().Select(method => method.Name).ToHashSet();

        methods.Should().Contain(methodName);
    }

    [Fact]
    public void IRepositoryBase_ShouldExposeExpectedMethodSignatures()
    {
        var type = typeof(IRepositoryBase<>);
        var methods = type.GetMethods().ToDictionary(method => method.Name, method => method);

        var findAll = methods["FindAll"];
        findAll.GetParameters().Should().BeEmpty();
        findAll.ReturnType.GetGenericTypeDefinition().Should().Be(typeof(IQueryable<>));

        var findByCondition = methods["FindByCondition"];
        var paramType = findByCondition.GetParameters().Single().ParameterType;
        paramType.GetGenericTypeDefinition().Should().Be(typeof(Expression<>));
        paramType.GenericTypeArguments[0].GetGenericTypeDefinition().Should().Be(typeof(Func<,>));

        methods["SaveAsync"].ReturnType.Should().Be(typeof(Task));
    }

    [Fact]
    public void IRepositoryBase_MoqImplementation_ShouldSupportCrudSetup()
    {
        var mock = new Mock<IRepositoryBase<FakeEntity>>();
        var data = new List<FakeEntity> { new() { Id = 1 }, new() { Id = 2 } }.AsQueryable();

        mock.Setup(repository => repository.FindAll()).Returns(data);
        mock.Setup(repository => repository.FindByCondition(It.IsAny<Expression<Func<FakeEntity, bool>>>() ))
            .Returns((Expression<Func<FakeEntity, bool>> expression) => data.Where(expression));
        mock.Setup(repository => repository.SaveAsync()).Returns(Task.CompletedTask);

        var all = mock.Object.FindAll().ToList();
        var filtered = mock.Object.FindByCondition(entity => entity.Id == 2).Single();

        all.Should().HaveCount(2);
        filtered.Id.Should().Be(2);
    }

    [Fact]
    public async Task IRepositoryBase_MoqImplementation_WhenSaveFails_ShouldPropagateException()
    {
        var mock = new Mock<IRepositoryBase<FakeEntity>>();
        mock.Setup(repository => repository.SaveAsync())
            .ThrowsAsync(new InvalidOperationException("Save failed"));

        Func<Task> action = async () => await mock.Object.SaveAsync();

        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("Save failed");
    }

    public sealed class FakeEntity
    {
        public int Id { get; set; }
    }
}
