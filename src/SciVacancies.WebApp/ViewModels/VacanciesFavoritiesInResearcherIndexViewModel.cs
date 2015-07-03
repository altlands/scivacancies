using SciVacancies.ReadModel.Core;
using SciVacancies.ReadModel.Pager;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacanciesFavoritiesInResearcherIndexViewModel : PageViewModelBase
    {
        public VacanciesFavoritiesInResearcherIndexViewModel()
        {
            Title = "Личная карточка пользователя";
        }
        public PagedList<Vacancy> Vacancies { get; set; }
    }
}
