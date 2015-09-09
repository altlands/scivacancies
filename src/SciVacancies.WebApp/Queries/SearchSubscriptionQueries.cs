using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Queries
{
    public class SingleSearchSubscriptionQuery : IRequest<SearchSubscription>
    {
        public Guid SearchSubscriptionGuid { get; set; }
    }
    public class SelectPagedSearchSubscriptionsQuery : IRequest<Page<SearchSubscription>>
    {
        public Guid ResearcherGuid { get; set; }

        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }

        //TODO - добавить фильтр по колонкам
    }
    public class SelectActiveSearchSubscriptionsQuery:IRequest<IEnumerable<SearchSubscription>>
    {
    }
}
