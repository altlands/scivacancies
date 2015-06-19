using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Enums
{
    public enum NotificationStatus
    {
        /// <summary>
        /// Создано, ещё не прочитано
        /// </summary>
        Created = 0,
        /// <summary>
        /// Прочитано
        /// </summary>
        Read = 1,
        /// <summary>
        /// Удалено
        /// </summary>
        Removed = 2
    }
}
