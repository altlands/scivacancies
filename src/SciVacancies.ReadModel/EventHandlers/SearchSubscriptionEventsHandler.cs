using MediatR;
using NPoco;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.ReadModel.EventHandlers
{
    public class SearchSubscriptionEventsHandler :
        INotificationHandler<SearchSubscriptionCreated>,
        INotificationHandler<SearchSubscriptionActivated>,
        INotificationHandler<SearchSubscriptionCanceled>,
        INotificationHandler<SearchSubscriptionRemoved>
    {
        private readonly IDatabase _db;

        public SearchSubscriptionEventsHandler(IDatabase db)
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

        public void Handle(SearchSubscriptionActivated msg)
        {
            SearchSubscription searchSubscription = _db.SingleById<SearchSubscription>(msg.SearchSubscriptionGuid);

            searchSubscription.UpdateDate = msg.TimeStamp;
            searchSubscription.Status = SearchSubscriptionStatus.Active;

            _db.Update(searchSubscription);
        }

        public void Handle(SearchSubscriptionCanceled msg)
        {
            SearchSubscription searchSubscription = _db.SingleById<SearchSubscription>(msg.SearchSubscriptionGuid);

            searchSubscription.UpdateDate = msg.TimeStamp;
            searchSubscription.Status = SearchSubscriptionStatus.Cancelled;

            _db.Update(searchSubscription);
        }

        public void Handle(SearchSubscriptionRemoved msg)
        {
            _db.Delete<SearchSubscription>(msg.SearchSubscriptionGuid);
        }
    }
}
