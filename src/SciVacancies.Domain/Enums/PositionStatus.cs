using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Enums
{
    public enum PositionStatus
    {
        /// <summary>
        /// В работе. По данной позиции опубликованной вакансии нет
        /// </summary>
        InProcess = 0,
        /// <summary>
        /// По данной позиции опубликована вакансия, работа с позицией не возможна до закрытия или отмены вакансии
        /// </summary>
        Published = 1,
        /// <summary>
        /// Позиция удалена и недоступна для организации (нужно создать новую)
        /// </summary>
        Removed = 2
    }
}
