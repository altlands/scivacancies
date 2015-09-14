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
            SearchSubscription searchSubscription = new SearchSubscription(Guid.NewGuid(), msg.ResearcherGuid, msg.Data);
            _repository.Save(searchSubscription, Guid.NewGuid(), null);

            return searchSubscription.Id;
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
            if (msg.SearchSubscriptionGuid == Guid.Empty) throw new ArgumentNullException($"SearchSubscriptionGuid is empty: {msg.SearchSubscriptionGuid}");

            SearchSubscription searchSubscription = _repository.GetById<SearchSubscription>(msg.SearchSubscriptionGuid);
            searchSubscription.Activate();
            _repository.Save(searchSubscription, Guid.NewGuid(), null);
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
            if (msg.SearchSubscriptionGuid == Guid.Empty) throw new ArgumentNullException($"SearchSubscriptionGuid is empty: {msg.SearchSubscriptionGuid}");

            SearchSubscription searchSubscription = _repository.GetById<SearchSubscription>(msg.SearchSubscriptionGuid);
            searchSubscription.Cancel();
            _repository.Save(searchSubscription, Guid.NewGuid(), null);
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
            if (msg.SearchSubscriptionGuid == Guid.Empty) throw new ArgumentNullException($"SearchSubscriptionGuid is empty: {msg.SearchSubscriptionGuid}");

            SearchSubscription searchSubscription = _repository.GetById<SearchSubscription>(msg.SearchSubscriptionGuid);
            searchSubscription.Remove();
            _repository.Save(searchSubscription, Guid.NewGuid(), null);
        }
    }
}