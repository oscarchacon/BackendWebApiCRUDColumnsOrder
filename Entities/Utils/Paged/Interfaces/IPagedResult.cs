using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Utils.Paged.Interfaces
{
    /// <summary>
    /// Interfaz para la clase del objeto de paginacion conteniendo los resultados
    /// </summary>
    /// <typeparam name="T">Clase de parametro a usarse</typeparam>
    public interface IPagedResult<T> : IPagedResultBase where T : class
    {
        IEnumerable<T> Results { get; set; }
    }
}
