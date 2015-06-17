using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class ApplicationCreateViewModel: PageViewModelBase
    {
        public Guid VacancyGuid { get; set; }
    }
}
