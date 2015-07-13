using System.ComponentModel;

namespace SciVacancies.Domain.Enums
{
    public enum ResearcherStatus
    {
        /// <summary>
        /// Исследователь создан/активен
        /// </summary>
        [Description("Активен")]
        Active = 0,

        /// <summary>
        /// Исследователь удалён
        /// </summary>
        [Description("Удалён")]
        Removed = 1,

        /// <summary>
        /// Исследователь заблокирован (блокировка\разблокировка доступна только администратору)
        /// //TODO - нужно ли это?
        /// </summary>
        [Description("Заблокирован")]
        Banned = 2
    }
}
