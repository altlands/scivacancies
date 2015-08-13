using System.Collections.Generic;
using SciVacancies.ReadModel.Core;
using SciVacancies.ReadModel.Pager;

namespace SciVacancies.WebApp.ViewModels
{
    public class SearchSubscriptionsInResearcherIndexViewModel
    {
        public PagedList<SearchSubscription> PagedItems { get; set; }
    }
}