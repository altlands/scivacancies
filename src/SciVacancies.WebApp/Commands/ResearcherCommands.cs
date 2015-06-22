using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class CreateResearcherCommand : CommandBase, IRequest<Guid>
    {
        public CreateResearcherCommand() : base() { }

        public ResearcherDataModel Data { get; set; }
    }
    public class UpdateResearcherCommand : CommandBase, IRequest
    {
        public UpdateResearcherCommand() : base() { }

        public Guid ResearcherGuid { get; set; }

        public ResearcherDataModel Data { get; set; }
    }
    public class RemoveResearcherCommand : CommandBase, IRequest
    {
        public RemoveResearcherCommand() : base() { }

        public Guid ResearcherGuid { get; set; }
    }
}