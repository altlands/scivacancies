using SciVacancies.Domain.Aggregates;

using System;

using CommonDomain.Persistence;
using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class CreateResearcherCommandHandler : IRequestHandler<CreateResearcherCommand, Guid>
    {
        private readonly IRepository _repository;

        public CreateResearcherCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public Guid Handle(CreateResearcherCommand msg)
        {
            Researcher researcher = new Researcher(Guid.NewGuid(), msg.Data);
            _repository.Save(researcher, Guid.NewGuid(), null);

            return researcher.Id;
        }
    }
    public class UpdateResearcherCommandHandler : RequestHandler<UpdateResearcherCommand>
    {
        private readonly IRepository _repository;

        public UpdateResearcherCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(UpdateResearcherCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            researcher.Update(msg.Data);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
    public class RemoveResearcherCommandHandler : RequestHandler<RemoveResearcherCommand>
    {
        private readonly IRepository _repository;

        public RemoveResearcherCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(RemoveResearcherCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            researcher.Remove();
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
}