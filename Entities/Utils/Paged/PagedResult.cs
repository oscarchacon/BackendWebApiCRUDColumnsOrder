using Entities.Utils.Paged.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Utils.Paged
{
    /// <summary>
    /// Class for pagination objects
    /// </summary>
    /// <typeparam name="T">Parameter class.</typeparam>
    public class PagedResult<T> : PagedResultBase, IPagedResult<T> where T : class
    {
        public IEnumerable<T> Results { get; set; }

        public PagedResult()
        {
            this.Results = new List<T>();
        }
    }
}
