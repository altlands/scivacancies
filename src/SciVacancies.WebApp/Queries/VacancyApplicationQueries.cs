using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Queries
{
    public class SingleVacancyApplicationQuery : IRequest<VacancyApplication>
    {
        public Guid VacancyApplicationGuid { get; set; }
    }
    public class SelectPagedVacancyApplicationsByResearcherQuery : IRequest<Page<VacancyApplication>>
    {
        public Guid ResearcherGuid { get; set; }

        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }

        //TODO - добавить фильтр сортировки по колонкам
    }
    public class SelectPagedVacancyApplicationsByVacancyQuery : IRequest<Page<VacancyApplication>>
    {
        public Guid VacancyGuid { get; set; }

        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }

        //TODO - добавить фильтр сортировки по колонкам
    }

    public class SelectVacancyApplicationAttachmentsQuery:IRequest<IEnumerable<VacancyApplicationAttachment>>
    {
        public Guid VacancyApplicationGuid { get; set; }
    }
}
