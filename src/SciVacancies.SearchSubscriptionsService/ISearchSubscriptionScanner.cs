using System.Collections.Generic;
using System.Threading;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.SearchSubscriptionsService
{
    public interface ISearchSubscriptionScanner
    {
        void PoolHandleSubscriptions(object threadContext);
        void Initialize(ManualResetEvent doneEvent, IEnumerable<SearchSubscription> subscriptionQueue);
    }
}