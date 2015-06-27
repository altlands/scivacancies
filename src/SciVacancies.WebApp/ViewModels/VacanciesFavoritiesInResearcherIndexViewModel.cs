using NPoco;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacanciesFavoritiesInResearcherIndexViewModel : PageViewModelBase
    {
        public VacanciesFavoritiesInResearcherIndexViewModel()
        {
            Title = "Личная карточка пользователя";
        }
        public Page<Vacancy> Vacancies { get; set; }
    }
}
