using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SingleResearcherQuery : IRequest<Researcher>
    {
        public Guid ResearcherGuid { get; set; }
    }

    public class SelectAllResearchersQuery : IRequest<List<Researcher>>
    {

    }
}
