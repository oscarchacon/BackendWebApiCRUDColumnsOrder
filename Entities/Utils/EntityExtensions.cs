using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.Utils
{
    /// <summary>
    /// Utility extension for model classes.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Detects if the object is null.
        /// </summary>
        /// <param name="entity">Model entity object</param>
        /// <returns>Returns true if the object is null</returns>
        public static bool IsObjectNull<T>(this T entity) where T : class
        {
            return entity == null;
        }

        /// <summary>
        /// Detects if the object is empty or has an empty ID.
        /// </summary>
        /// <param name="entity">Model entity object</param>
        /// <returns>Returns true if the object is empty</returns>
        public static bool IsEmptyObject(this IEntity entity)
        {
            return entity.Id.Equals(Guid.Empty);
        }

        /// <summary>
        /// Detects if the object list is null.
        /// </summary>
        /// <param name="entityList">Model entity list</param>
        /// <returns>Returns true if the object list is null</returns>
        public static bool IsListObjectNull<T>(this IEnumerable<T> entityList) where T : class
        {
            return entityList == null;
        }

        /// <summary>
        /// Detects if the object list is empty.
        /// </summary>
        /// <param name="entityList">Model entity list</param>
        /// <returns>Returns true if the object list is empty</returns>
        public static bool IsEmptyListObject<T>(this IEnumerable<T> entityList) where T : class
        {
            return !entityList.Any();
        }

        /// <summary>
        /// Detects if the structure list is null.
        /// </summary>
        /// <param name="entityList">Structure list</param>
        /// <returns>Returns true if the structure list is null</returns>
        public static bool IsListNull<T>(this IEnumerable<T> entityList) where T : struct
        {
            return entityList == null;
        }

        /// <summary>
        /// Detects if the structure list is empty.
        /// </summary>
        /// <param name="entityList">Structure list</param>
        /// <returns>Returns true if the structure list is empty</returns>
        public static bool IsEmptyList<T>(this IEnumerable<T> entityList) where T : struct
        {
            return !entityList.Any();
        }
    }
}
