using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Utils
{
    /// <summary>
    /// Clase extensión de Utilidad para el uso en los controladores.
    /// </summary>
    public static class ClassUtils
    {
        /// <summary>
        /// Función que permite saber si la Lista tiene contenido
        /// </summary>
        /// <returns>Retorna booleano si hay contenido en la lista</returns>
        /// <param name="data">Lista IEnumerable</param>
        /// <typeparam name="T">Clase que contiene la lista</typeparam>
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }

        /// <summary>
        /// Función que permite saber si la Lista tiene contenido
        /// </summary>
        /// <returns>Retorna booleano si hay contenido en la lista</returns>
        /// <param name="data">Lista IList</param>
        /// <typeparam name="T">Clase que contiene la lista</typeparam>
        public static bool IsAny<T>(this IList<T> data)
        {
            return data != null && data.Any();
        }

        /// <summary>
        /// Función que permite saber si el objeto es nulo
        /// </summary>
        /// <typeparam name="T">Clase se contiene el Objeto</typeparam>
        /// <param name="data">Objeto</param>
        /// <returns>Retorna booleano si el objeto es nulo</returns>
        public static bool IsNull<T>(this T data) where T : class
        {
            return data == null;
        }
    }
}
