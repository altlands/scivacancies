using System.Collections.Generic;

namespace SciVacancies.ReadModel.Pager
{
    public interface IPagedList<T> : IPagedList
    {
        List<T> Items { get; set; }
    }

    public interface IPagedList
    {
        int TotalItems { get; set; }
        int TotalPages { get; set; }
        int CurrentPage { get; set; }
        int PageSize { get; set; }
        string SortField { get; set; }
        string SortDirection { get; set; }
    }
}
