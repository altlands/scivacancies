using SciVacancies.ReadModel.Core;

using System;

using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SingleSearchSubscriptionQueryHandler : IRequestHandler<SingleSearchSubscriptionQuery, SearchSubscription>
    {
        private readonly IDatabase _db;

        public SingleSearchSubscriptionQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public SearchSubscription Handle(SingleSearchSubscriptionQuery message)
        {
            if (message.SearchSubscriptionGuid == Guid.Empty) throw new ArgumentNullException($"SearchSubscriptionGuid is empty: {message.SearchSubscriptionGuid}");

            SearchSubscription searchSubscription = _db.SingleOrDefaultById<SearchSubscription>(message.SearchSubscriptionGuid);

            return searchSubscription;
        }
    }
    public class SelectPagedSearchSubscriptionsQueryHandler : IRequestHandler<SelectPagedSearchSubscriptionsQuery, Page<SearchSubscription>>
    {
        private readonly IDatabase _db;

        public SelectPagedSearchSubscriptionsQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Page<SearchSubscription> Handle(SelectPagedSearchSubscriptionsQuery message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");

            //TODO - фильтрация и сортировка

            Page<SearchSubscription> searchSubscriptions = _db.Page<SearchSubscription>(message.PageIndex, message.PageSize, new Sql("SELECT s.* FROM \"SearchSubscriptions\" s WHERE s.\"ResearcherGuid\"=" + message.ResearcherGuid + " ORDER BY s.\"Guid\" DESC"));

            return searchSubscriptions;
        }
    }
}