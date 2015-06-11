using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class ResearcherEditViewModel: PageViewModelBase
    {
        public ResearcherEditViewModel()
        {
            NavigationTitle = "Редактировать";
            Title = "Редактирование информации пользователя";
        }
    }
}
