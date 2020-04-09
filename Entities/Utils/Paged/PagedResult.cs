using Entities.Utils.Paged.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Utils.Paged
{
    /// <summary>
    /// Clase para ser usada en los objetos de paginación
    /// </summary>
    /// <typeparam name="T">Clase de parámetro.</typeparam>
    public class PagedResult<T> : PagedResultBase, IPagedResult<T> where T : class
    {
        public IEnumerable<T> Results { get; set; }

        public PagedResult()
        {
            this.Results = new List<T>();
        }
    }
}
