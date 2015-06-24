using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Queries
{
    public class SingleVacancyQuery : IRequest<Vacancy>
    {
        public Guid VacancyGuid { get; set; }
    }
    public class SelectVacanciesByOrganizationQuery : IRequest<List<Vacancy>>
    {
        public Guid OrganizationGuid { get; set; }
    }
    public class SelectVacanciesByTitleQuery : IRequest<List<Vacancy>>
    {
        public string Title { get; set; }
        public int Count { get; set; }
    }
    public class SelectPagedVacanciesQuery : IRequest<Page<Vacancy>>
    {
        public string OrderBy { get; set; }
        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string NameFilterValue { get; set; }
        public string AddressFilterValue { get; set; }
    }
    public class SelectClosedVacanciesByOrganizationQuery : IRequest<List<Vacancy>>
    {
        public Guid OrganizationGuid { get; set; }
    }
    public class SelectPagedClosedVacanciesQuery : IRequest<Page<Vacancy>>
    {
        public string OrderBy { get; set; }
        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string NameFilterValue { get; set; }
        public string AddressFilterValue { get; set; }
    }
    public class SelectFavoriteVacanciesByResearcherQuery : IRequest<List<Vacancy>>
    {
        public Guid ResearcherGuid { get; set; }
    }
}