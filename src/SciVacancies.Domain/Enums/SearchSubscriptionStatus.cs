using System.ComponentModel;

namespace SciVacancies.Domain.Enums
{
    public enum SearchSubscriptionStatus
    {
        /// <summary>
        /// Подписка активна
        /// </summary>
        [Description("Активна")]
        Active = 0,

        /// <summary>
        /// Подписка отменена
        /// </summary>
        [Description("Неактивна")]
        Cancelled = 1,

        /// <summary>
        /// Подписка удалена
        /// </summary>
        [Description("Удалена")]
        Removed = 2
    }
}
