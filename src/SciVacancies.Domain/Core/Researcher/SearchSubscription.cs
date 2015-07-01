using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;

using System;

namespace SciVacancies.Domain.Core
{
    public class SearchSubscription
    {
        /// <summary>
        /// Guid поисковой подписки
        /// </summary>
        public Guid SearchSubscriptionGuid { get; set; }

        /// <summary>
        /// Вся информация о поисковой подписке
        /// </summary>
        public SearchSubscriptionDataModel Data { get; set; }

        /// <summary>
        /// Статус поисковой подписки
        /// </summary>
        public SearchSubscriptionStatus Status { get; set; }
    }
}
