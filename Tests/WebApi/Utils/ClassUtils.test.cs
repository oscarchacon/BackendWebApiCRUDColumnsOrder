using FluentAssertions;
using WebApi.Utils;

namespace Tests.WebApi.Utils;

public class ClassUtilsTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", true)]
    [InlineData("value", true)]
    public void IsAny_ForIEnumerable_ShouldReturnExpectedResult(string? value, bool expected)
    {
        IEnumerable<string>? data = value is null ? null : new[] { value };

        var result = data.IsAny();

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    public void IsAny_ForIList_ShouldReturnExpectedResult(int count, bool expected)
    {
        IList<int>? data = count == 0 ? new List<int>() : new List<int> { 1 };

        data.IsAny().Should().Be(expected);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsNull_ShouldReturnExpectedResult(bool isNull)
    {
        var value = isNull ? null : "x";

        value.IsNull().Should().Be(isNull);
    }
}
