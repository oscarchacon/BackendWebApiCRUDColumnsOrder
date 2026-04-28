using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Utils
{
    /// <summary>
    /// Utility extension class for controllers.
    /// </summary>
    public static class ClassUtils
    {
        /// <summary>
        /// Checks if the list has content.
        /// </summary>
        /// <returns>Returns true if the list contains elements.</returns>
        /// <param name="data">IEnumerable list</param>
        /// <typeparam name="T">List item type</typeparam>
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }

        /// <summary>
        /// Checks if the list has content.
        /// </summary>
        /// <returns>Returns true if the list contains elements.</returns>
        /// <param name="data">IList list</param>
        /// <typeparam name="T">List item type</typeparam>
        public static bool IsAny<T>(this IList<T> data)
        {
            return data != null && data.Any();
        }

        /// <summary>
        /// Checks if the object is null.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="data">Object</param>
        /// <returns>Returns true if the object is null</returns>
        public static bool IsNull<T>(this T data) where T : class
        {
            return data == null;
        }
    }
}
