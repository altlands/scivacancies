using System;
using SciVacancies.Domain.DataModels;

namespace SciVacancies.Domain.Events
{
    public class SearchSubscriptionEventBase : EventBase
    {
        public Guid SearchSubscriptionGuid { get; set; }
        public Guid ResearcherGuid { get; set; }
    }

    /// <summary>
    /// Подписка создана и активна
    /// </summary>
    public class SearchSubscriptionCreated : SearchSubscriptionEventBase
    {
        public SearchSubscriptionDataModel Data { get; set; }
    }

    /// <summary>
    /// Подписка активна
    /// </summary>
    public class SearchSubscriptionActivated : SearchSubscriptionEventBase
    {
    }

    /// <summary>
    /// Подписка отменена, есть возможность активировать повторно
    /// </summary>
    public class SearchSubscriptionCanceled : SearchSubscriptionEventBase
    {
    }

    /// <summary>
    /// Подписка удалена, более недоступна
    /// </summary>
    public class SearchSubscriptionRemoved : SearchSubscriptionEventBase
    {
    }
}
