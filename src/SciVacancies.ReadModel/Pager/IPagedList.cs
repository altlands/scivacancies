using System.Collections.Generic;

namespace SciVacancies.ReadModel.Pager
{
    public interface IPagedList<T> : IPagedList
    {
        IList<T> Items { get; set; }
    }

    public interface IPagedList
    {
        int TotalItems { get; set; }
        int TotalPages { get; set; }
        int CurrentPage { get; set; }
        int PageSize { get; set; }
    }
}
