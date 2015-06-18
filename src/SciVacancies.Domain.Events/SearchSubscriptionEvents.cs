using SciVacancies.Domain.DataModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Events
{
    public class SearchSubscriptionEventBase : EventBase
    {
        public SearchSubscriptionEventBase() : base() { }

        public Guid SearchSubscriptionGuid { get; set; }
        public Guid ResearcherGuid { get; set; }
    }
    public class SearchSubscriptionCreated : SearchSubscriptionEventBase
    {
        public SearchSubscriptionCreated() : base() { }

        public SearchSubscriptionDataModel Data { get; set; }
    }
    public class SearchSubscriptionActivated : SearchSubscriptionEventBase
    {
        public SearchSubscriptionActivated() : base() { }
    }
    public class SearchSubscriptionCanceled : SearchSubscriptionEventBase
    {
        public SearchSubscriptionCanceled() : base() { }
    }
    public class SearchSubscriptionRemoved : SearchSubscriptionEventBase
    {
        public SearchSubscriptionRemoved() : base() { }
    }
}
