using SciVacancies.ReadModel.Core;

using System;

using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SingleResearcherQuery : IRequest<Researcher>
    {
        public Guid ResearcherGuid { get; set; }
    }
}
