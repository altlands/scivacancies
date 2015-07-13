using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using MediatR;
using NPoco;
using AutoMapper;

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
            SearchSubscription searchSubscription = Mapper.Map<SearchSubscription>(msg);

            using (var transaction = _db.GetTransaction())
            {
                _db.Insert(searchSubscription);
                transaction.Complete();
            }
        }
        public void Handle(SearchSubscriptionActivated msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE res_searchsubscriptions SET status = @0, update_date = @1 WHERE guid = @2", SearchSubscriptionStatus.Active, msg.TimeStamp, msg.SearchSubscriptionGuid));
                transaction.Complete();
            }
        }
        public void Handle(SearchSubscriptionCanceled msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE res_searchsubscriptions SET status = @0, update_date = @1 WHERE guid = @2", SearchSubscriptionStatus.Cancelled, msg.TimeStamp, msg.SearchSubscriptionGuid));
                transaction.Complete();
            }
        }
        public void Handle(SearchSubscriptionRemoved msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE res_searchsubscriptions SET status = @0, update_date = @1 WHERE guid = @2", SearchSubscriptionStatus.Removed, msg.TimeStamp, msg.SearchSubscriptionGuid));
                transaction.Complete();
            }
        }
    }
}
