using SciVacancies.ReadModel.Pager;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacancyApplicationsInResearcherIndexViewModel : PageViewModelBase
    {
        public VacancyApplicationsInResearcherIndexViewModel()
        {
            Title = "Личная карточка пользователя";
        }
        public PagedList<VacancyApplicationDetailsViewModel> Applications { get; set; }
    }
}
