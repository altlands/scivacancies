using SciVacancies.Domain.Aggregates;

using System;

using AutoMapper;
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

        public Guid Handle(CreateResearcherCommand message)
        {
            var rdm = message.Data;

            Researcher researcher = new Researcher(Guid.NewGuid(), rdm);
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

        protected override void HandleCore(UpdateResearcherCommand message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");

            var rdm = message.Data;

            Researcher researcher = _repository.GetById<Researcher>(message.ResearcherGuid);
            researcher.Update(rdm);
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

        protected override void HandleCore(RemoveResearcherCommand message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");

            Researcher researcher = _repository.GetById<Researcher>(message.ResearcherGuid);
            researcher.Remove();
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
}