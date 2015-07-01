using System.ComponentModel;
using System;

namespace SciVacancies.Domain.Enums
{
    public enum VacancyStatus
    {
        /// <summary>
        /// Вакансию можно редактировать, видна только организации
        /// </summary>
        [Description("В работе")]
        InProcess = 0,

        /// <summary>
        /// Вакансия опубликована, но приём заявок открыт
        /// </summary>
        [Description("Объявлена")]
        Published = 1,

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
        Cancelled = 4,

        /// <summary>
        /// Контракт подписан с победителем или претендентом
        /// </summary>
        [Description("Контракт подписан")]
        OfferAccepted = 5,

        /// <summary>
        /// И победитель и претендент отказались от подписания контракта
        /// </summary>
        [Description("Контракт отклонён")]
        OfferRejected = 6,

        /// <summary>
        /// Вакансия удалена и недоступна для организации (вакансию можно удалить, если она ещё не была опубликована)
        /// </summary>
        [Description("Удалена")]
        Removed = 7,

        /// <summary>
        /// Приём заявок.
        /// </summary>
        [Obsolete("Will be removed")]
        AppliesAcceptance = 8
    }
}
