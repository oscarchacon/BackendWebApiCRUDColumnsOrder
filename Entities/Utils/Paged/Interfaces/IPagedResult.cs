using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Utils.Paged.Interfaces
{
    /// <summary>
    /// Interface for the pagination result object
    /// </summary>
    /// <typeparam name="T">Parameter class to use</typeparam>
    public interface IPagedResult<T> : IPagedResultBase where T : class
    {
        IEnumerable<T> Results { get; set; }
    }
}
