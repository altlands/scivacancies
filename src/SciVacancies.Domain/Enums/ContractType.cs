using System.ComponentModel;

namespace SciVacancies.Domain.Enums
{
    public enum ContractType
    {
        /// <summary>
        /// Бессрочный
        /// </summary>
        [Description("Бессрочный")]
        Permanent = 0,
        /// <summary>
        /// Фиксированный срок
        /// </summary>
        [Description("Срочный")]
        FixedTerm = 1
    }
}
