using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Utils
{
    /// <summary>
    /// Utility class for entity properties.
    /// </summary>
    public static class EntityProperties
    {
        /// <summary>
        /// Method that determines if a property exists by its name.
        /// </summary>
        /// <param name="typeEntity">Entity object type</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Returns true if the property exists</returns>
        public static bool ContainsPropertyName(Type typeEntity, string propertyName)
        {
            try
            {
                var properties = typeEntity.GetProperties();
                var propertyInfo = Array.Find(properties, propertyClass => propertyClass.Name.ToLower().Equals(propertyName.ToLower()));
                return propertyInfo != null;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
