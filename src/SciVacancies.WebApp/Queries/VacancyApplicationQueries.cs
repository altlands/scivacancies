using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;
using NPoco;
using SciVacancies.Domain.Enums;

namespace SciVacancies.WebApp.Queries
{
    public class SingleVacancyApplicationQuery : IRequest<VacancyApplication>
    {
        public Guid VacancyApplicationGuid { get; set; }
    }
    public class CountVacancyApplicationInVacancyQuery : IRequest<int>
    {
        public Guid VacancyGuid { get; set; }
        public VacancyApplicationStatus Status { get; set; }
    }
    public class SelectPagedVacancyApplicationsByResearcherQuery : IRequest<Page<VacancyApplication>>
    {
        public Guid ResearcherGuid { get; set; }

        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }

        //TODO - добавить фильтр по колонкам
    }
    public class SelectVacancyApplicationsByResearcherQuery : IRequest<IEnumerable<VacancyApplication>>
    {
        public Guid ResearcherGuid { get; set; }
    }
    public class SelectPagedVacancyApplicationsByVacancyQuery : IRequest<Page<VacancyApplication>>
    {
        public Guid VacancyGuid { get; set; }

        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }

        //TODO - добавить фильтр по колонкам
    }
    public class SelectAllVacancyApplicationAttachmentsQuery : IRequest<IEnumerable<VacancyApplicationAttachment>>
    {
        public Guid VacancyApplicationGuid { get; set; }
    }
}
