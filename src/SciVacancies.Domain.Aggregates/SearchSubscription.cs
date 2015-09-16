using CommonDomain.Core;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using System;

namespace SciVacancies.Domain.Aggregates
{
    public class SearchSubscription : AggregateBase
    {
        public Guid ResearcherGuid { get; private set; }

        private SearchSubscriptionDataModel Data { get; set; }

        public SearchSubscriptionStatus Status { get; private set; }

        public SearchSubscription()
        {

        }
        public SearchSubscription(Guid guid, Guid researcherGuid, SearchSubscriptionDataModel data)
        {
            if (guid.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(guid));
            if (researcherGuid.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(researcherGuid));
            if (data == null) throw new ArgumentNullException(nameof(data));

            RaiseEvent(new SearchSubscriptionCreated
            {
                SearchSubscriptionGuid = guid,
                ResearcherGuid = researcherGuid,
                Data = data
            });
        }

        #region Methods

        public void Activate()
        {
            if (Status != SearchSubscriptionStatus.Cancelled) throw new InvalidOperationException("searchSubscription state is invalid");

            RaiseEvent(new SearchSubscriptionActivated
            {
                SearchSubscriptionGuid = Id,
                ResearcherGuid = ResearcherGuid
            });
        }
        public void Cancel()
        {
            if (Status != SearchSubscriptionStatus.Active) throw new InvalidOperationException("searchSubscription state is invalid");

            RaiseEvent(new SearchSubscriptionCanceled
            {
                SearchSubscriptionGuid = Id,
                ResearcherGuid = ResearcherGuid
            });
        }
        public void Remove()
        {
            if (Status == SearchSubscriptionStatus.Removed) throw new InvalidOperationException("searchSubscription state is invalid");

            RaiseEvent(new SearchSubscriptionRemoved
            {
                SearchSubscriptionGuid = Id,
                ResearcherGuid = ResearcherGuid
            });
        }
        #endregion

        #region Apply-Handlers

        public void Apply(SearchSubscriptionCreated @event)
        {
            Id = @event.SearchSubscriptionGuid;
            ResearcherGuid = @event.ResearcherGuid;
            Data = @event.Data;
        }
        public void Apply(SearchSubscriptionActivated @event)
        {
            Status = SearchSubscriptionStatus.Active;
        }
        public void Apply(SearchSubscriptionCanceled @event)
        {
            Status = SearchSubscriptionStatus.Cancelled;
        }
        public void Apply(SearchSubscriptionRemoved @event)
        {
            Status = SearchSubscriptionStatus.Removed;
        }

        #endregion
    }
}
