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
        
        /// <summary>
        /// Поданные Исследователем вакансии. Используется для выбора какие действия показывать Исследователю, если он подал или не подал Заявку на Вакансию
        /// </summary>
        public List<VacancyApplication> AppliedApplications { get; set; } = new List<VacancyApplication>();
    }
}
