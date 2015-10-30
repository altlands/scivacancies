using System;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public static class SagaDateTimeExtensions
    {
        /// <summary>
        /// Добавить период ожидания ответа для Победителя
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime AddPeriodToOfferResponseAwaitingFromWinner(this DateTime source)
        {
            //return source.AddDays(30);
            return source.AddMinutes(5);
        }
        /// <summary>
        /// Добавить период ожидания ответа для Претендента
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime AddPeriodToOfferResponseAwaitingFromPretender(this DateTime source)
        {
            //return source.AddDays(30);
            return source.AddMinutes(5);
        }
    }
}
