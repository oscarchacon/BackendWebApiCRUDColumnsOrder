using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    /// <summary>
    /// Interfaz para que sea de base para todas las clases del modelo.
    /// </summary>
    public interface IEntity
    {
        [Key]
        Guid Id { get; set; }
    }
}
