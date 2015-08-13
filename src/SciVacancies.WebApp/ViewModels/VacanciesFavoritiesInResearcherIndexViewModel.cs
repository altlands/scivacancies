using System.Collections.Generic;
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
        public List<VacancyApplication> AppliedApplications { get; set; } = new List<VacancyApplication>();
    }
}
