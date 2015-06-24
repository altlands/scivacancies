using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SingleSearchSubscriptionQuery : IRequest<SearchSubscription>
    {
        public Guid SearchSubscriptionGuid { get; set; }
    }
    public class SelectAllSearchSubscriptions : IRequest<List<SearchSubscription>>
    {

    }
    public class SelectSearchSubscriptionsByResearcher : IRequest<List<SearchSubscription>>
    {
        public Guid ResearcherGuid { get; set; }
    }
}
