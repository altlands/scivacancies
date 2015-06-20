using System.Collections.Generic;
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
        public IEnumerable<Vacancy> Vacancies { get; set; }
    }
}
