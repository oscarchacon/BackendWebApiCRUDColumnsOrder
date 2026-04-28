using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Utils.Paged.Interfaces
{
    /// <summary>
    /// Base interface for pagination result objects.
    /// </summary>
    public interface IPagedResultBase
    {
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        int PageSize { get; set; }
        int RowCount { get; set; }
        int FirstRowOnPage { get; }
        int LastRowOnPage { get; }
    }
}
