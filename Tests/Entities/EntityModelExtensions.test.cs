using Entities.Extensions;
using Entities.Models;
using FluentAssertions;

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

        dbEntity.Name.Should().Be("Updated");
        dbEntity.Description.Should().Be("Updated description");
        dbEntity.RegisterDate.Should().Be(originalDate);
    }

    [Fact]
    public void Map_WhenIncomingEntityIsNull_ThrowsNullReferenceException()
    {
        var dbEntity = new Entity();

        Action act = () => dbEntity.Map(null!);

        act.Should().Throw<NullReferenceException>();
    }
}
