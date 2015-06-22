using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.Domain.Commands
{
    public class CreateResearcher : CommandBase, IRequest<Guid>
    {
        public CreateResearcher() : base() { }

        public ResearcherDataModel Data { get; set; }
    }
    public class UpdateResearcher : CommandBase, IRequest
    {
        public UpdateResearcher() : base() { }

        public Guid ResearcherGuid { get; set; }

        public ResearcherDataModel Data { get; set; }
    }
    public class RemoveResearcher : CommandBase, IRequest
    {
        public RemoveResearcher() : base() { }

        public Guid ResearcherGuid { get; set; }
    }
}