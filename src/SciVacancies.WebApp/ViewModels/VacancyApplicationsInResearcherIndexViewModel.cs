using NPoco;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacancyApplicationsInResearcherIndexViewModel : PageViewModelBase
    {
        public VacancyApplicationsInResearcherIndexViewModel()
        {
            Title = "Личная карточка пользователя";
        }
        public Page<VacancyApplicationDetailsViewModel> Applications { get; set; }
    }
}
