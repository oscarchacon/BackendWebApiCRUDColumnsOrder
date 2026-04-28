using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    /// <summary>
    /// Interface that serves as the base for all model classes.
    /// </summary>
    public interface IEntity
    {
        [Key]
        Guid Id { get; set; }
    }
}
