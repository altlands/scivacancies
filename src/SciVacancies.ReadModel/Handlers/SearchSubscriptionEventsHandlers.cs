using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Handlers
{
    public class SearchSubscriptionCreatedHandler : EventBaseHandler<SearchSubscriptionCreated>
    {
        public SearchSubscriptionCreatedHandler(IDatabase db) : base(db) { }
        public override void Handle(SearchSubscriptionCreated msg)
        {
            SearchSubscription searchSubscription = new SearchSubscription()
            {
                Status = SearchSubscriptionStatus.Active
            };

            _db.Insert(searchSubscription);
        }
    }
    public class SearchSubscriptionActivatedHandler : EventBaseHandler<SearchSubscriptionActivated>
    {
        public SearchSubscriptionActivatedHandler(IDatabase db) : base(db) { }
        public override void Handle(SearchSubscriptionActivated msg)
        {
            SearchSubscription searchSubscription = _db.SingleById<SearchSubscription>(msg.SearchSubscriptionGuid);

            searchSubscription.Status = SearchSubscriptionStatus.Active;

            _db.Update(searchSubscription);
        }
    }
    public class SearchSubscriptionCanceledHandler : EventBaseHandler<SearchSubscriptionCanceled>
    {
        public SearchSubscriptionCanceledHandler(IDatabase db) : base(db) { }
        public override void Handle(SearchSubscriptionCanceled msg)
        {
            SearchSubscription searchSubscription = _db.SingleById<SearchSubscription>(msg.SearchSubscriptionGuid);

            searchSubscription.Status = SearchSubscriptionStatus.Cancelled;

            _db.Update(searchSubscription);
        }
    }
    public class SearchSubscriptionRemovedHandler : EventBaseHandler<SearchSubscriptionRemoved>
    {
        public SearchSubscriptionRemovedHandler(IDatabase db) : base(db) { }
        public override void Handle(SearchSubscriptionRemoved msg)
        {
            _db.Delete<SearchSubscription>(msg.SearchSubscriptionGuid);
        }
    }
}
