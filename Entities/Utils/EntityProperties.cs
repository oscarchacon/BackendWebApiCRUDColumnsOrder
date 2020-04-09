using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Utils
{
    /// <summary>
    /// Clase de Útilidad para las propiedades de las Entidades.
    /// </summary>
    public static class EntityProperties
    {
        /// <summary>
        /// Método que permite saber si contiene un propiedad por su nombre
        /// </summary>
        /// <param name="typeEntity">Tipo de Objeto de Entidad </param>
        /// <param name="propertyName">Nombre de Propiedad</param>
        /// <returns>Booleano si contiene la propiedad</returns>
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
