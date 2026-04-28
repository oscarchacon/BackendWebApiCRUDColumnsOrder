using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Repository.Utils;

namespace Tests.Repository;

public class RepositoryExtensionTests
{
    [Fact]
    public void GetPaged_WhenCalled_ReturnsExpectedMetadataAndRows()
    {
        var source = Enumerable.Range(1, 10).Select(v => new SampleRow { Value = v }).AsQueryable();

        var result = source.GetPaged(page: 2, pageSize: 3);

        result.CurrentPage.Should().Be(2);
        result.PageSize.Should().Be(3);
        result.RowCount.Should().Be(10);
        result.PageCount.Should().Be(4);
        result.Results.Select(x => x.Value).Should().Equal(new[] { 4, 5, 6 });
    }

    [Fact]
    public async Task GetPagedAsync_WhenCalled_ReturnsExpectedMetadataAndRows()
    {
        using var context = BuildContext();
        await SeedAsync(context, 7);
        var source = context.SampleRows.OrderBy(x => x.Value);

        var result = await source.GetPagedAsync(page: 2, pageSize: 3);

        result.CurrentPage.Should().Be(2);
        result.PageSize.Should().Be(3);
        result.RowCount.Should().Be(7);
        result.PageCount.Should().Be(3);
        result.Results.Select(x => x.Value).Should().Equal(new[] { 4, 5, 6 });
    }

    [Fact]
    public async Task GetPagedListAsync_WhenCalled_ReturnsOnlyRequestedSlice()
    {
        using var context = BuildContext();
        await SeedAsync(context, 9);
        var source = context.SampleRows.OrderBy(x => x.Value);

        var result = (await source.GetPagedListAsync(page: 3, pageSize: 2)).ToList();

        result.Select(x => x.Value).Should().Equal(new[] { 5, 6 });
    }

    private sealed class SampleRow
    {
        public int Id { get; set; }
        public int Value { get; set; }
    }

    private sealed class TestPagingContext : DbContext
    {
        public TestPagingContext(DbContextOptions<TestPagingContext> options) : base(options)
        {
        }

        public DbSet<SampleRow> SampleRows => Set<SampleRow>();
    }

    private static TestPagingContext BuildContext()
    {
        var options = new DbContextOptionsBuilder<TestPagingContext>()
            .UseInMemoryDatabase($"repository-extension-tests-{Guid.NewGuid()}")
            .Options;

        return new TestPagingContext(options);
    }

    private static async Task SeedAsync(TestPagingContext context, int count)
    {
        var rows = Enumerable.Range(1, count).Select(v => new SampleRow { Value = v });
        await context.SampleRows.AddRangeAsync(rows);
        await context.SaveChangesAsync();
    }
}
