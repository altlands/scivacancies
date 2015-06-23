using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SingleVacancyApplicationQuery : IRequest<VacancyApplication>
    {
        public Guid VacancyApplicationGuid { get; set; }
    }
    public class SelectVacancyApplicationsByResearcherQuery : IRequest<List<VacancyApplication>>
    {
        public Guid ResearcherGuid { get; set; }
    }
    public class SelectVacancyApplicationsByVacancyQuery : IRequest<List<VacancyApplication>>
    {
        public Guid VacancyGuid { get; set; }
    }

}
