
using System.ComponentModel;

namespace SciVacancies.Domain.Enums
{
    public enum VacancyStatus
    {
        /// <summary>
        /// Вакансия опубликована, но приём заявок закрыт
        /// </summary>
        [Description("Объявлена")]
        Published = 0,
        /// <summary>
        /// Открыт приём заявок
        /// </summary>
        [Description("Приём заявок")]
        AppliesAcceptance = 1,
        /// <summary>
        /// Приём заявок закрыт, заявки на вакансию рассматриваются комиссией
        /// </summary>
        [Description("На рассмотрении")]
        InCommittee = 2,
        /// <summary>
        /// Вакансия закрыта, результаты объявлены
        /// </summary>
        [Description("Закрыта")]
        Closed = 3,
        /// <summary>
        /// Вакансия отменена
        /// </summary>
        [Description("Отменена")]
        Cancelled = 4
    }
}
