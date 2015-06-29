using NPoco;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.WebApp.ViewModels
{
    public class SearchSubscriptionsInResearcherIndexViewModel
    {
        public Page<SearchSubscription> PagedItems { get; set; }
    }
}