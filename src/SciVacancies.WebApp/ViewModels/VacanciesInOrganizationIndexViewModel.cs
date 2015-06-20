using System;
using System.Collections.ObjectModel;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacanciesInOrganizationIndexViewModel: Collection<Position>
    {
        public Guid OrganizationGuid { get; set; }

    }
}
