using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SearchSubscriptionQueriesHandler :
        IRequestHandler<SingleSearchSubscriptionQuery, SearchSubscription>,
        IRequestHandler<SelectPagedSearchSubscriptionsQuery, Page<SearchSubscription>>,
        IRequestHandler<SelectActiveSearchSubscriptionsQuery, IEnumerable<SearchSubscription>>

    {
        private readonly IDatabase _db;

        public SearchSubscriptionQueriesHandler(IDatabase db)
        {
            _db = db;
        }

        public SearchSubscription Handle(SingleSearchSubscriptionQuery message)
        {
            if (message.SearchSubscriptionGuid == Guid.Empty) throw new ArgumentNullException($"SearchSubscriptionGuid is empty: {message.SearchSubscriptionGuid}");

            SearchSubscription searchSubscription = _db.SingleOrDefault<SearchSubscription>(new Sql($"SELECT s.* FROM res_searchsubscriptions s WHERE s.guid = @0 AND s.status != @1", message.SearchSubscriptionGuid, SearchSubscriptionStatus.Removed));

            return searchSubscription;
        }
        public Page<SearchSubscription> Handle(SelectPagedSearchSubscriptionsQuery message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");

            //TODO - фильтрация и сортировка

            Page<SearchSubscription> searchSubscriptions = _db.Page<SearchSubscription>(message.PageIndex, message.PageSize, new Sql($"SELECT s.* FROM res_searchsubscriptions s WHERE s.researcher_guid = @0 AND s.status != @1 ORDER BY s.guid DESC", message.ResearcherGuid, SearchSubscriptionStatus.Removed));

            return searchSubscriptions;
        }
        public IEnumerable<SearchSubscription> Handle(SelectActiveSearchSubscriptionsQuery msg)
        {
            IEnumerable<SearchSubscription> searchsubscriptions = _db.Fetch<SearchSubscription>(new Sql($"SELECT * FROM res_searchsubscriptions ss WHERE ss.status = @0 ORDER BY ss.processed_date DESC", SearchSubscriptionStatus.Active));

            return searchsubscriptions;
        }
    }
}