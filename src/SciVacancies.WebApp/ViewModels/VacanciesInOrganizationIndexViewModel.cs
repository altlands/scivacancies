using System;
using SciVacancies.ReadModel.Core;
using SciVacancies.ReadModel.Pager;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacanciesInOrganizationIndexViewModel {

        public Guid OrganizationGuid { get; set; }

        public PagedList<Vacancy> PagedVacancies { get; set; }

        public string Name { get; set; }
    }
}
