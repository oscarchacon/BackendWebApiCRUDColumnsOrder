using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Extensions
{
    /// <summary>
    /// Clase de Extensión para la Clase Entidad
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Método que mapea un objeto de Entidad dentro de otro objeto Entidad
        /// </summary>
        /// <param name="dbEntity">Objeto Entidad a mapear con los datos del otro objeto</param>
        /// <param name="entity">Objeto Entidad con datos a mapear</param>
        public static void Map(this Entity dbEntity, Entity entity)
        {
            dbEntity.Name = entity.Name;
            dbEntity.Description = entity.Description;
        }
    }
}
