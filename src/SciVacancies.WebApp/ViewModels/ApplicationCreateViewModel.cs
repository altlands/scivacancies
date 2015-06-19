using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Rendering;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class ApplicationCreateViewModel : PageViewModelBase
    {
        public ApplicationCreateViewModel() { }

        public ApplicationCreateViewModel(Guid organizationGuid)
        {
            OrganizationGuid = organizationGuid;
        }

        public Guid VacancyGuid { get; set; }
        public Guid OrganizationGuid { get; set; }

        public ApplicationCreateViewModel InitDictionaries(IReadModelService readModelService)
        {
            if (readModelService==null)
                throw new ArgumentNullException(nameof(readModelService), "Ошибка при получении сервиса");

            if (OrganizationGuid == Guid.Empty)
                throw new NullReferenceException($"Для выполненеия метода требуется {nameof(OrganizationGuid)}");

            PositionTypes = readModelService.SelectPositionTypes().Select(c=> new SelectListItem {Value = c.Id.ToString(), Text = c.Title});


            return this;
        }

        public string PositionType { get; set; }
        public IEnumerable<SelectListItem> PositionTypes { get; set; }
    }
}
