using SciVacancies.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using CommonDomain.Persistence;
using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class CreateSearchSubscriptionCommandHandler : IRequestHandler<CreateSearchSubscriptionCommand, Guid>
    {
        private readonly IRepository _repository;

        public CreateSearchSubscriptionCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public Guid Handle(CreateSearchSubscriptionCommand message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new Exception($"ResearcherGuid is empty: {message.ResearcherGuid}");

            var rdm = message.Data;

            Researcher researcher = _repository.GetById<Researcher>(message.ResearcherGuid);
            Guid searchSubscriptionGuid = researcher.CreateSearchSubscription(rdm);
            _repository.Save(researcher, Guid.NewGuid(), null);

            return searchSubscriptionGuid;
        }
    }
    public class ActivateSearchSubscriptionCommandHandler : RequestHandler<ActivateSearchSubscriptionCommand>
    {
        private readonly IRepository _repository;

        public ActivateSearchSubscriptionCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(ActivateSearchSubscriptionCommand message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new Exception($"ResearcherGuid is empty: {message.ResearcherGuid}");
            if (message.SearchSubscriptionGuid == Guid.Empty) throw new Exception($"SearchSubscriptionGuid is empty: {message.SearchSubscriptionGuid}");

            Researcher researcher = _repository.GetById<Researcher>(message.ResearcherGuid);
            researcher.ActivateSearchSubscription(message.SearchSubscriptionGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
    public class CancelSearchSubscriptionCommandHandler : RequestHandler<CancelSearchSubscriptionCommand>
    {
        private readonly IRepository _repository;

        public CancelSearchSubscriptionCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(CancelSearchSubscriptionCommand message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new Exception($"ResearcherGuid is empty: {message.ResearcherGuid}");
            if (message.SearchSubscriptionGuid == Guid.Empty) throw new Exception($"SearchSubscriptionGuid is empty: {message.SearchSubscriptionGuid}");

            Researcher researcher = _repository.GetById<Researcher>(message.ResearcherGuid);
            researcher.CancelSearchSubscription(message.SearchSubscriptionGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
    public class RemoveSearchSubscriptionCommandHandler : RequestHandler<RemoveSearchSubscriptionCommand>
    {
        private readonly IRepository _repository;

        public RemoveSearchSubscriptionCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(RemoveSearchSubscriptionCommand message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new Exception($"ResearcherGuid is empty: {message.ResearcherGuid}");
            if (message.SearchSubscriptionGuid == Guid.Empty) throw new Exception($"SearchSubscriptionGuid is empty: {message.SearchSubscriptionGuid}");

            Researcher researcher = _repository.GetById<Researcher>(message.ResearcherGuid);
            researcher.RemoveSearchSubscription(message.SearchSubscriptionGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
}