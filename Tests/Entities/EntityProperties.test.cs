using Entities.Models;
using Entities.Utils;
using FluentAssertions;

namespace Tests.Entities;

public class EntityPropertiesTests
{
    [Fact]
    public void ContainsPropertyName_WhenPropertyExists_ReturnsTrue()
    {
        var result = EntityProperties.ContainsPropertyName(typeof(Entity), "Name");

        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsPropertyName_WhenPropertyExistsWithDifferentCase_ReturnsTrue()
    {
        var result = EntityProperties.ContainsPropertyName(typeof(Entity), "registerdate");

        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsPropertyName_WhenPropertyDoesNotExist_ReturnsFalse()
    {
        var result = EntityProperties.ContainsPropertyName(typeof(Entity), "UnknownProperty");

        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsPropertyName_WhenTypeIsNull_ReturnsFalse()
    {
        var result = EntityProperties.ContainsPropertyName(null!, "Name");

        result.Should().BeFalse();
    }
}
