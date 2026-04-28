using FluentAssertions;
using Repository.Utils;

namespace Tests.Repository;

public class EFExtensionsTests
{
    [Fact]
    public async Task ToListAsyncSafe_WhenSourceIsNull_ThrowsArgumentNullException()
    {
        IQueryable<int>? source = null;

        Func<Task> action = async () => await source!.ToListAsyncSafe();

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ToListAsyncSafe_WhenSourceIsNotAsyncQueryable_ReturnsMaterializedList()
    {
        var source = new[] { 1, 2, 3 }.AsQueryable();

        var result = await source.ToListAsyncSafe();

        result.Should().Equal(new[] { 1, 2, 3 });
    }

    [Fact]
    public void CustomOrderBy_WhenPropertyDoesNotExist_ThrowsException()
    {
        var source = new[]
        {
            new SampleRow { Name = "B" },
            new SampleRow { Name = "A" }
        }.AsQueryable();

        Action act = () => source.CustomOrderBy("DoesNotExist").ToList();

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void CustomOrderByDescending_WhenPropertyDoesNotExist_ThrowsException()
    {
        var source = new[]
        {
            new SampleRow { Name = "B" },
            new SampleRow { Name = "A" }
        }.AsQueryable();

        Action act = () => source.CustomOrderByDescending("DoesNotExist").ToList();

        act.Should().Throw<Exception>();
    }

    private sealed class SampleRow
    {
        public string Name { get; set; } = string.Empty;
    }
}
