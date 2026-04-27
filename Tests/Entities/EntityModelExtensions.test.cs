using Entities.Extensions;
using Entities.Models;

namespace Tests.Entities;

public class EntityModelExtensionsTests
{
    [Fact]
    public void Map_WhenCalled_UpdatesNameAndDescriptionOnly()
    {
        var originalDate = DateTime.UtcNow.AddDays(-7);
        var dbEntity = new Entity
        {
            Id = Guid.NewGuid(),
            Name = "Original",
            Description = "Original description",
            RegisterDate = originalDate
        };

        var incoming = new Entity
        {
            Name = "Updated",
            Description = "Updated description",
            RegisterDate = DateTime.UtcNow
        };

        dbEntity.Map(incoming);

        Assert.Equal("Updated", dbEntity.Name);
        Assert.Equal("Updated description", dbEntity.Description);
        Assert.Equal(originalDate, dbEntity.RegisterDate);
    }

    [Fact]
    public void Map_WhenIncomingEntityIsNull_ThrowsNullReferenceException()
    {
        var dbEntity = new Entity();

        Assert.Throws<NullReferenceException>(() => dbEntity.Map(null!));
    }
}
