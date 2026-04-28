using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Extensions
{
    /// <summary>
    /// Extension class for the Entity class.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Method that maps one entity object into another entity object.
        /// </summary>
        /// <param name="dbEntity">Entity object to be mapped with the other object's data</param>
        /// <param name="entity">Entity object with data to map</param>
        public static void Map(this Entity dbEntity, Entity entity)
        {
            dbEntity.Name = entity.Name;
            dbEntity.Description = entity.Description;
        }
    }
}
