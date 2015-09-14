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
        /// Ожидается решение победителя или претендента (будет подписывать контракт или нет).
        /// </summary>
        [Obsolete("now use OfferResponseFromWinnerAwaiting || OfferResponseFromPretenderAwaiting")]
        [Description("Предложение контракта")]
        OfferResponseAwaiting = 3,

        /// <summary>
        /// Ожидается решение от победителя
        /// </summary>
        [Description("Предложение контракта победителю")]
        OfferResponseFromWinnerAwaiting = 4,

        /// <summary>
        /// Победитель согласился подписать контракт
        /// </summary>
        [Description("Предложение принято победителем")]
        OfferAcceptedByWinner = 5,

        /// <summary>
        /// Победитель отказался подписать контракт или вовремя не нажал кнопку
        /// </summary>
        [Description("Предложение отклонено победителем")]
        OfferRejectedByWinner = 6,

        /// <summary>
        /// Ожидается решение от претендента
        /// </summary>
        [Description("Предложение контракта претенденту")]
        OfferResponseFromPretenderAwaiting = 7,

        /// <summary>
        /// Претендент согласился подписать контракт
        /// </summary>
        [Description("Предложение принято претендентом")]
        OfferResponseAcceptedByPretender = 8,

        /// <summary>
        /// Претендент отказался подписать контракт
        /// </summary>
        [Description("Предложение отклонено претендентом")]
        OfferResponseRejectedByPretender = 9,

        /// <summary>
        /// Контракт подписан с победителем или претендентом
        /// </summary>
        [Obsolete("now use OfferAcceptedByWinner || OfferResponseAcceptedByPretender")]
        [Description("Предложение принято")]
        OfferAccepted = 10,

        /// <summary>
        /// Победитель или претендент отказались от подписания контракта
        /// </summary>
        [Obsolete("now use OfferRejectedByWinner || OfferResponseRejectedByPretender")]
        [Description("Контракт отклонён")]
        OfferRejected = 11,

        /// <summary>
        /// Вакансия закрыта, результаты объявлены
        /// </summary>
        [Description("Закрыта")]
        Closed = 12,

        /// <summary>
        /// Вакансия отменена
        /// </summary>
        [Description("Отменена")]
        Cancelled = 13,

        /// <summary>
        /// Вакансия удалена и недоступна для организации (вакансию можно удалить, если она ещё не была опубликована)
        /// </summary>
        [Description("Удалена")]
        Removed = 14
    }
}
