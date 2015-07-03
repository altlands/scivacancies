using System.Collections.Generic;

namespace SciVacancies.ReadModel.Pager
{

    public class PagedList<T>: PagedList, IPagedList<T>
    {
        public List<T> Items { get; set; }
    }

    public class PagedList: IPagedList
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int FirstRowIndexOnPage { get; set; }
        public int LastRowIndexOnPage { get; set; }
    }
}
