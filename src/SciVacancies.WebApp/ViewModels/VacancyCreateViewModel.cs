using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc.Rendering;
using NPoco.Expressions;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacancyCreateViewModel : PageViewModelBase
    {
        public VacancyCreateViewModel() { }

        public VacancyCreateViewModel(Guid organizationGuid)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            OrganizationGuid = organizationGuid;
        }

        public Guid OrganizationGuid { get; set; }


        /// <summary>
        /// Сохранить как черновик (true) или сохранить и опубликовать данные (false)
        /// </summary>
        public bool ToPublish { get; set; }

        public VacancyCreateViewModel InitDictionaries(IReadModelService readModelService)
        {
            if (readModelService == null)
                throw new ArgumentNullException(nameof(readModelService));
            if (OrganizationGuid == Guid.Empty)
                throw new NullReferenceException($"Property {nameof(OrganizationGuid)} has Empty value");

            PositionTypes = readModelService.SelectPositionTypes().Select(c => new SelectListItem { Text = c.Title, Value = c.Guid.ToString() });

            return this;
        }


        public IEnumerable<SelectListItem> PositionTypes { get; set; }

        /// <summary>
        /// Guid должности из справочника
        /// </summary>
        public Guid PositionTypeGuid { get; set; }
    }
}