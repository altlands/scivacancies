using System.ComponentModel;

namespace SciVacancies.Domain.Enums
{
    public enum OrganizationStatus
    {
        /// <summary>
        /// Организация создана/активна
        /// </summary>
        [Description("Активна")]
        Active = 0,

        /// <summary>
        /// Организация удалена
        /// </summary>
        [Description("Удалена")]
        Removed = 1,

        /// <summary>
        /// Организация заблокирована (блокировка\разблокировка доступна только администратору)
        /// //TODO - нужно ли это?
        /// </summary>
        [Description("Заблокирована")]
        Banned = 2
    }
}
