using System.Collections.Generic;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacancyApplicationsInResearcherIndexViewModel : PageViewModelBase
    {
        public VacancyApplicationsInResearcherIndexViewModel()
        {
            Title = "Личная карточка пользователя";
        }
        public IEnumerable<VacancyApplication> Applications { get; set; }
    }
}
