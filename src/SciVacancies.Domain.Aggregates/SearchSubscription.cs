using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;

using System;

using CommonDomain.Core;

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
            if (guid.Equals(Guid.Empty)) throw new ArgumentNullException("guid is empty");
            if (researcherGuid.Equals(Guid.Empty)) throw new ArgumentNullException("researcherGuid is empty");
            if (data == null) throw new ArgumentNullException("data is empty");

            RaiseEvent(new SearchSubscriptionCreated()
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

            RaiseEvent(new SearchSubscriptionActivated()
            {
                SearchSubscriptionGuid = Id,
                ResearcherGuid = ResearcherGuid
            });
        }
        public void Cancel()
        {
            if (Status != SearchSubscriptionStatus.Active) throw new InvalidOperationException("searchSubscription state is invalid");

            RaiseEvent(new SearchSubscriptionCanceled()
            {
                SearchSubscriptionGuid = Id,
                ResearcherGuid = ResearcherGuid
            });
        }
        public void Remove()
        {
            if (Status == SearchSubscriptionStatus.Removed) throw new InvalidOperationException("searchSubscription state is invalid");

            RaiseEvent(new SearchSubscriptionRemoved()
            {
                SearchSubscriptionGuid = Id,
                ResearcherGuid = ResearcherGuid
            });
        }
        #endregion

        #region Apply-Handlers

        public void Apply(SearchSubscriptionCreated @event)
        {
            this.Id = @event.SearchSubscriptionGuid;
            this.ResearcherGuid = @event.ResearcherGuid;
            this.Data = @event.Data;
            this.Status = SearchSubscriptionStatus.Active;
        }
        public void Apply(SearchSubscriptionActivated @event)
        {
            this.Status = SearchSubscriptionStatus.Active;
        }
        public void Apply(SearchSubscriptionCanceled @event)
        {
            this.Status = SearchSubscriptionStatus.Cancelled;
        }
        public void Apply(SearchSubscriptionRemoved @event)
        {
            this.Status = SearchSubscriptionStatus.Removed;
        }

        #endregion
    }
}
