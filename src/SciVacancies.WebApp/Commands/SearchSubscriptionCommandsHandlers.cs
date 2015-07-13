using SciVacancies.Domain.Aggregates;

using System;

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

        public Guid Handle(CreateSearchSubscriptionCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            Guid searchSubscriptionGuid = researcher.CreateSearchSubscription(msg.Data);
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

        protected override void HandleCore(ActivateSearchSubscriptionCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            if (msg.SearchSubscriptionGuid == Guid.Empty) throw new ArgumentNullException($"SearchSubscriptionGuid is empty: {msg.SearchSubscriptionGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            researcher.ActivateSearchSubscription(msg.SearchSubscriptionGuid);
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

        protected override void HandleCore(CancelSearchSubscriptionCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            if (msg.SearchSubscriptionGuid == Guid.Empty) throw new ArgumentNullException($"SearchSubscriptionGuid is empty: {msg.SearchSubscriptionGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            researcher.CancelSearchSubscription(msg.SearchSubscriptionGuid);
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

        protected override void HandleCore(RemoveSearchSubscriptionCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            if (msg.SearchSubscriptionGuid == Guid.Empty) throw new ArgumentNullException($"SearchSubscriptionGuid is empty: {msg.SearchSubscriptionGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            researcher.RemoveSearchSubscription(msg.SearchSubscriptionGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
}