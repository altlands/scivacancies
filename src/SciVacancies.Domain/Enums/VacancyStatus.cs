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
        /// Ожидается решение победителя (будет подписывать контракт или нет).
        /// Если победитель отказался, то ожидается решение претендента
        /// </summary>
        [Description("Предложение контракта")]
        OfferResponseAwaiting = 3,

        /// <summary>
        /// Контракт подписан с победителем или претендентом
        /// </summary>
        [Description("Контракт подписан")]
        OfferAccepted = 4,

        /// <summary>
        /// И победитель и претендент отказались от подписания контракта
        /// </summary>
        [Description("Контракт отклонён")]
        OfferRejected = 5,

        /// <summary>
        /// Вакансия закрыта, результаты объявлены
        /// </summary>
        [Description("Закрыта")]
        Closed = 6,

        /// <summary>
        /// Вакансия отменена
        /// </summary>
        [Description("Отменена")]
        Cancelled = 7,

        /// <summary>
        /// Вакансия удалена и недоступна для организации (вакансию можно удалить, если она ещё не была опубликована)
        /// </summary>
        [Description("Удалена")]
        Removed = 8
    }
}
