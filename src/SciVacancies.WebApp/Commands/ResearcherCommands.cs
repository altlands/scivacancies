using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class CreateResearcherCommand : CommandBase, IRequest<Guid>
    {
        public ResearcherDataModel Data { get; set; }
    }
    public class UpdateResearcherCommand : CommandBase, IRequest
    {
        public Guid ResearcherGuid { get; set; }

        public ResearcherDataModel Data { get; set; }
    }
    public class RemoveResearcherCommand : CommandBase, IRequest
    {
        public Guid ResearcherGuid { get; set; }
    }
}