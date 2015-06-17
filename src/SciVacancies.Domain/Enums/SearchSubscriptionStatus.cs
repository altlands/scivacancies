using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Enums
{
    public enum SearchSubscriptionStatus
    {
        /// <summary>
        /// Подписка активна
        /// </summary>
        Active = 0,
        /// <summary>
        /// Подписка отменена
        /// </summary>
        Cancelled = 1
    }
}
