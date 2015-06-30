using System.ComponentModel;

namespace SciVacancies.Domain.Enums
{
    public enum OrganizationStatus
    {
        /// <summary>
        /// Организация создана/активна
        /// </summary>
        Active = 0,
        /// <summary>
        /// Организация заблокирована (блокировка\разблокировка доступна только администратору)
        /// </summary>
        Banned = 1,
        /// <summary>
        /// Организация удалена
        /// </summary>
        Removed = 2
    }
}
