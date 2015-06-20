using System;
using System.Collections.Generic;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacanciesInOrganizationIndexViewModel
    {
        public Guid OrganizationGuid { get; set; }

        public IEnumerable<Vacancy> Vacancies { get; set; }
        public IEnumerable<Position> Positions { get; set; }

    }
}
