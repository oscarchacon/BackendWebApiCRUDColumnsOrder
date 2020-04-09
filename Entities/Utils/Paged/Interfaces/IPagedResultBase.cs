using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Utils.Paged.Interfaces
{
    /// <summary>
    /// Interface Base para el Objeto de Paginación
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
