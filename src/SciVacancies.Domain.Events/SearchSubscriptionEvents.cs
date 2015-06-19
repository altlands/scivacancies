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
    /// <summary>
    /// Подписка создана и активна.
    /// </summary>
    public class SearchSubscriptionCreated : SearchSubscriptionEventBase
    {
        public SearchSubscriptionCreated() : base() { }

        public SearchSubscriptionDataModel Data { get; set; }
    }
    /// <summary>
    /// Подписка активна
    /// </summary>
    public class SearchSubscriptionActivated : SearchSubscriptionEventBase
    {
        public SearchSubscriptionActivated() : base() { }
    }
    /// <summary>
    /// Подписка отменена, есть возможность активировать повторно
    /// </summary>
    public class SearchSubscriptionCanceled : SearchSubscriptionEventBase
    {
        public SearchSubscriptionCanceled() : base() { }
    }
    /// <summary>
    /// Подписка удалена, более недоступна
    /// </summary>
    public class SearchSubscriptionRemoved : SearchSubscriptionEventBase
    {
        public SearchSubscriptionRemoved() : base() { }
    }
}
