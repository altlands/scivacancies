using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

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
