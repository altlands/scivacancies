using System.ComponentModel;

namespace SciVacancies.Domain.Enums
{
    public enum ResearcherStatus
    {
        /// <summary>
        /// Исследователь создан/активен
        /// </summary>
        Active = 0,
        /// <summary>
        /// Исследователь заблокирован (блокировка\разблокировка доступна только администратору)
        /// </summary>
        Banned = 1,
        /// <summary>
        /// Исследователь удалён
        /// </summary>
        Removed = 2
    }
}
