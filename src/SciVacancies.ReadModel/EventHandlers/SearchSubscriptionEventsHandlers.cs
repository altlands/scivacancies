using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;
using MediatR;

namespace SciVacancies.ReadModel.EventHandlers
{
    public class SearchSubscriptionCreatedHandler : INotificationHandler<SearchSubscriptionCreated>
    {
        private readonly IDatabase _db;

        public SearchSubscriptionCreatedHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(SearchSubscriptionCreated msg)
        {
            SearchSubscription searchSubscription = new SearchSubscription()
            {
                Guid = msg.SearchSubscriptionGuid,
                ResearcherGuid = msg.ResearcherGuid,
                CreationDate = msg.TimeStamp,
                Status = SearchSubscriptionStatus.Active
            };

            _db.Insert(searchSubscription);
        }
    }
    public class SearchSubscriptionActivatedHandler : INotificationHandler<SearchSubscriptionActivated>
    {
        private readonly IDatabase _db;

        public SearchSubscriptionActivatedHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(SearchSubscriptionActivated msg)
        {
            SearchSubscription searchSubscription = _db.SingleById<SearchSubscription>(msg.SearchSubscriptionGuid);

            searchSubscription.UpdateDate = msg.TimeStamp;
            searchSubscription.Status = SearchSubscriptionStatus.Active;

            _db.Update(searchSubscription);
        }
    }
    public class SearchSubscriptionCanceledHandler : INotificationHandler<SearchSubscriptionCanceled>
    {
        private readonly IDatabase _db;

        public SearchSubscriptionCanceledHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(SearchSubscriptionCanceled msg)
        {
            SearchSubscription searchSubscription = _db.SingleById<SearchSubscription>(msg.SearchSubscriptionGuid);

            searchSubscription.UpdateDate = msg.TimeStamp;
            searchSubscription.Status = SearchSubscriptionStatus.Cancelled;

            _db.Update(searchSubscription);
        }
    }
    public class SearchSubscriptionRemovedHandler : INotificationHandler<SearchSubscriptionRemoved>
    {
        private readonly IDatabase _db;

        public SearchSubscriptionRemovedHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(SearchSubscriptionRemoved msg)
        {
            _db.Delete<SearchSubscription>(msg.SearchSubscriptionGuid);
        }
    }
}
