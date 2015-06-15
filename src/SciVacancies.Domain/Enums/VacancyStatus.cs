using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Enums
{
    public enum VacancyStatus
    {
        /// <summary>
        /// Вакансия опубликована, но приём заявок закрыт
        /// </summary>
        Published = 0,
        /// <summary>
        /// Открыт приём заявок
        /// </summary>
        AppliesAcceptance = 1,
        /// <summary>
        /// Приём заявок закрыт, заявки на вакансию рассматриваются комиссией
        /// </summary>
        InCommittee = 2,
        /// <summary>
        /// Вакансия закрыта, результаты объявлены
        /// </summary>
        Closed = 3,
        /// <summary>
        /// Вакансия отменена
        /// </summary>
        Cancelled = 4
    }
}
