using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.Utils
{
    /// <summary>
    /// Extensión de utilidad para clases pertenecientes al modelo
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Función que detecta si el objeto es nulo
        /// </summary>
        /// <param name="entity">Objeto de Entidad de Modelo</param>
        /// <returns>Booleano si el objeto es nulo</returns>
        public static bool IsObjectNull<T>(this T entity) where T : class
        {
            return entity == null;
        }

        /// <summary>
        /// Función que detecta si el objeto viene sin contenido o su id es vacío
        /// </summary>
        /// <param name="entity">Objeto de Entidad de Modelo</param>
        /// <returns>Booleano si el objeto es vacio</returns>
        public static bool IsEmptyObject(this IEntity entity)
        {
            return entity.Id.Equals(Guid.Empty);
        }

        /// <summary>
        /// Función que detecta si la lista de objetos es nula
        /// </summary>
        /// <param name="entityList">Lista de Objetos de Entidad de Modelo</param>
        /// <returns>Booleano si la lista de objetos es nulo</returns>
        public static bool IsListObjectNull<T>(this IEnumerable<T> entityList) where T : class
        {
            return entityList == null;
        }

        /// <summary>
        /// Función que detecta si la lista objetos es vacia
        /// </summary>
        /// <param name="entityList">Lista de Objeto de Entidad de Modelo</param>
        /// <returns>Booleano si la lista de objetos es vacia</returns>
        public static bool IsEmptyListObject<T>(this IEnumerable<T> entityList) where T : class
        {
            return !entityList.Any();
        }

        /// <summary>
        /// Función que detecta si la lista de estructura es nula
        /// </summary>
        /// <param name="entityList">Lista de estructura</param>
        /// <returns>Booleano si la lista de estructura es nula</returns>
        public static bool IsListNull<T>(this IEnumerable<T> entityList) where T : struct
        {
            return entityList == null;
        }

        /// <summary>
        /// Función que detecta si la lista estructura es vacia
        /// </summary>
        /// <param name="entityList">Lista de estructura</param>
        /// <returns>Booleano si la lista de estructura es vacia</returns>
        public static bool IsEmptyList<T>(this IEnumerable<T> entityList) where T : struct
        {
            return !entityList.Any();
        }
    }
}
