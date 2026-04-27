using Repository.Utils;

namespace Tests.Repository;

public class RepositoryExtensionTests
{
    [Fact]
    public void GetPaged_WhenCalled_ReturnsExpectedPageMetadataAndRows()
    {
        var source = Enumerable.Range(1, 10).Select(value => new SampleRow { Value = value }).AsQueryable();

        var result = source.GetPaged(page: 2, pageSize: 3);

        Assert.Equal(2, result.CurrentPage);
        Assert.Equal(3, result.PageSize);
        Assert.Equal(10, result.RowCount);
        Assert.Equal(4, result.PageCount);
        Assert.Equal(new[] { 4, 5, 6 }, result.Results.Select(row => row.Value).ToArray());
    }

    [Fact]
    public void GetPagedList_WhenCalled_ReturnsOnlyRequestedSlice()
    {
        var source = Enumerable.Range(1, 8).Select(value => new SampleRow { Value = value }).AsQueryable();

        var result = source.GetPagedList(page: 3, pageSize: 2).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(new[] { 5, 6 }, result.Select(row => row.Value).ToArray());
    }

    private sealed class SampleRow
    {
        public int Value { get; set; }
    }
}
