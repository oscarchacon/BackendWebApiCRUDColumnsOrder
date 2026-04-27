using Entities.Models;
using Entities.Utils;

namespace Tests.Entities;

public class EntityPropertiesTests
{
    [Fact]
    public void ContainsPropertyName_WhenPropertyExists_ReturnsTrue()
    {
        var result = EntityProperties.ContainsPropertyName(typeof(Entity), "Name");

        Assert.True(result);
    }

    [Fact]
    public void ContainsPropertyName_WhenPropertyExistsWithDifferentCase_ReturnsTrue()
    {
        var result = EntityProperties.ContainsPropertyName(typeof(Entity), "registerdate");

        Assert.True(result);
    }

    [Fact]
    public void ContainsPropertyName_WhenPropertyDoesNotExist_ReturnsFalse()
    {
        var result = EntityProperties.ContainsPropertyName(typeof(Entity), "UnknownProperty");

        Assert.False(result);
    }

    [Fact]
    public void ContainsPropertyName_WhenTypeIsNull_ReturnsFalse()
    {
        var result = EntityProperties.ContainsPropertyName(null!, "Name");

        Assert.False(result);
    }
}
