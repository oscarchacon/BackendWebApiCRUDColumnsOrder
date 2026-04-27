using System;

namespace Entities.DTO;

public class EntityDTO : IEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}
