using Repository.Utils;

namespace Tests.Repository;

public class EFExtensionsTests
{
    [Fact]
    public async Task ToListAsyncSafe_WhenSourceIsNull_ThrowsArgumentNullException()
    {
        IQueryable<int>? source = null;

        await Assert.ThrowsAsync<ArgumentNullException>(() => source!.ToListAsyncSafe());
    }

    [Fact]
    public async Task ToListAsyncSafe_WhenSourceIsNotAsyncQueryable_ReturnsMaterializedList()
    {
        var source = new[] { 1, 2, 3 }.AsQueryable();

        var result = await source.ToListAsyncSafe();

        Assert.Equal(new[] { 1, 2, 3 }, result);
    }

    [Fact]
    public void CustomOrderBy_WhenPropertyDoesNotExist_ThrowsException()
    {
        var source = new[]
        {
            new SampleRow { Name = "B" },
            new SampleRow { Name = "A" }
        }.AsQueryable();

        Assert.ThrowsAny<Exception>(() => source.CustomOrderBy("DoesNotExist").ToList());
    }

    [Fact]
    public void CustomOrderByDescending_WhenPropertyDoesNotExist_ThrowsException()
    {
        var source = new[]
        {
            new SampleRow { Name = "B" },
            new SampleRow { Name = "A" }
        }.AsQueryable();

        Assert.ThrowsAny<Exception>(() => source.CustomOrderByDescending("DoesNotExist").ToList());
    }

    private sealed class SampleRow
    {
        public string Name { get; set; } = string.Empty;
    }
}
