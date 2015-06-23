using System.Collections.Generic;

namespace SciVacancies.WebApp.Engine.Pager
{

    public class PagedList<T>: PagedList, IPagedList<T>
    {
        public IList<T> Items { get; set; }
    }

    public class PagedList: IPagedList
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
