using System;
using System.Collections.Generic;
using NPoco;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacanciesInOrganizationIndexViewModel {

        public Guid OrganizationGuid { get; set; }

        public IEnumerable<Vacancy> Vacancies { get; set; }
        public IEnumerable<Position> Positions { get; set; }

        public Page<Vacancy> PagedVacancies { get; set; }
        public Page<Position> PagedPositions { get; set; }

        public string Name { get; set; }
    }
}
