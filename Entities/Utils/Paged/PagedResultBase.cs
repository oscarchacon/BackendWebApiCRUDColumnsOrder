using Entities.Utils.Paged.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Utils.Paged
{
    /// <summary>
    /// Base class for pagination result objects.
    /// </summary>
    public class PagedResultBase : IPagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {
            get => (CurrentPage - 1) * PageSize + 1;
        }

        public int LastRowOnPage
        {
            get => Math.Min(CurrentPage * PageSize, RowCount);
        }
    }
}
