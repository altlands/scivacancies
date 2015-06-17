using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Enums
{
    public enum VacancyApplicationStatus
    {
        /// <summary>
        /// Заявка на вакансию создана, но не отправлена
        /// </summary>
        InProcess = 0,
        /// <summary>
        /// Заявка отправлена
        /// </summary>
        Applied = 1,
        /// <summary>
        /// Заявка отменена
        /// </summary>
        Cancelled = 2,
        /// <summary>
        /// Заявка "выйграла" вакансию
        /// </summary>
        Won = 3,
        /// <summary>
        /// Заявка "получила" второе место
        /// </summary>
        Pretended = 4,
        /// <summary>
        /// Заявка "проиграла" конкурс
        /// </summary>
        Lost = 5
    }
}
