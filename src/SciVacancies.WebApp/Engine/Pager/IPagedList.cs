using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.WebApp.Engine.Pager
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
