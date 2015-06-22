using System.ComponentModel;

namespace SciVacancies.Domain.Enums
{
    public enum NotificationStatus
    {
        /// <summary>
        /// Создано, ещё не прочитано
        /// </summary>
        [Description("Не прочитано")]
        Created = 0,
        /// <summary>
        /// Прочитано
        /// </summary>
        [Description("Прочитано")]
        Read = 1,
        /// <summary>
        /// Удалено
        /// </summary>
        [Description("Удалено")]
        Removed = 2
    }
}
