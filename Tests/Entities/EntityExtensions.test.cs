using Entities.Models;
using Entities.Utils;

namespace Tests.Entities;

public class EntityExtensionsTests
{
    [Fact]
    public void IsObjectNull_WhenReferenceIsNull_ReturnsTrue()
    {
        string? value = null;

        var result = value.IsObjectNull();

        Assert.True(result);
    }

    [Fact]
    public void IsEmptyObject_WhenIdIsEmpty_ReturnsTrue()
    {
        var entity = new Entity { Id = Guid.Empty };

        var result = entity.IsEmptyObject();

        Assert.True(result);
    }

    [Fact]
    public void IsEmptyObject_WhenIdHasValue_ReturnsFalse()
    {
        var entity = new Entity { Id = Guid.NewGuid() };

        var result = entity.IsEmptyObject();

        Assert.False(result);
    }

    [Fact]
    public void IsListObjectNull_WhenListIsNull_ReturnsTrue()
    {
        List<Entity>? entities = null;

        var result = entities.IsListObjectNull();

        Assert.True(result);
    }

    [Fact]
    public void IsEmptyListObject_WhenListIsEmpty_ReturnsTrue()
    {
        var entities = new List<Entity>();

        var result = entities.IsEmptyListObject();

        Assert.True(result);
    }

    [Fact]
    public void IsEmptyListObject_WhenListIsNull_ThrowsArgumentNullException()
    {
        List<Entity>? entities = null;

        Assert.Throws<ArgumentNullException>(() => entities!.IsEmptyListObject());
    }
}
