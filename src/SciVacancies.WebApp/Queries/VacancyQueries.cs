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
    public class SelectPagedVacanciesQuery : IRequest<Page<Vacancy>>
    {
        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }

        /// <summary>
        /// Получить вакансии доступные для поиска
        /// </summary>
        public bool PublishedOnly { get; set; }

        public string NameFilterValue { get; set; }
        public string AddressFilterValue { get; set; }
    }
    public class SelectPagedVacanciesByOrganizationQuery : IRequest<Page<Vacancy>>
    {
        public Guid OrganizationGuid { get; set; }

        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }

        //public string NameFilterValue { get; set; }
        //public string AddressFilterValue { get; set; }
    }
    public class SelectVacanciesForAutocompleteQuery : IRequest<IEnumerable<Vacancy>>
    {
        public string Query { get; set; }
        public int Take { get; set; }
    }
    public class SelectPagedClosedVacanciesByOrganizationQuery : IRequest<Page<Vacancy>>
    {
        public Guid OrganizationGuid { get; set; }

        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }

        //public string NameFilterValue { get; set; }
        //public string AddressFilterValue { get; set; }
    }
    public class SelectPagedFavoriteVacanciesByResearcherQuery : IRequest<Page<Vacancy>>
    {
        public Guid ResearcherGuid { get; set; }

        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }

        //public string NameFilterValue { get; set; }
        //public string AddressFilterValue { get; set; }
    }
    public class SelectPagedVacanciesByGuidsQuery : IRequest<Page<Vacancy>>
    {
        public IEnumerable<Guid> VacancyGuids { get; set; }

        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }

        //public string NameFilterValue { get; set; }
        //public string AddressFilterValue { get; set; }
    }

    public class SelectVacancyCriteriasQuery : IRequest<IEnumerable<VacancyCriteria>>
    {
        public Guid VacancyGuid { get; set; }
    }
    public class SelectVacancyAttachmentsQuery : IRequest<IEnumerable<VacancyAttachment>>
    {
        public Guid VacancyGuid { get; set; }
    }
}