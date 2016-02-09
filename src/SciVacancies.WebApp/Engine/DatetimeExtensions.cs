using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;

namespace SciVacancies.WebApp
{
    public static class DatetimeExtensions
    {
        /// <summary>
        /// показать дату в формате для сайта
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToVacancyDateString(this DateTime source)
        {
            return $"{source.Day.ToString("00")}-{source.Month.ToString("00")}-{source.Year.ToString("0000")}";
        }
        /// <summary>
        /// показать дату и время в формате для сайта
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToVacancyDateTimeString(this DateTime source)
        {
            return $"{source.Day.ToString("00")}-{source.Month.ToString("00")}-{source.Year.ToString("0000")}, {source.Hour.ToString("00")}:{source.Minute.ToString("00")}";
        }
        /// <summary>
        /// показать дату и время в формате для сайта в московском часовом поясе
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToLocalMoscowVacancyDateTimeString(this DateTime source)
        {
            var mscSource = source.AddHours(3);
            return $"{mscSource.Day.ToString("00")}-{mscSource.Month.ToString("00")}-{mscSource.Year.ToString("0000")}, {mscSource.Hour.ToString("00")}:{mscSource.Minute.ToString("00")}";
        }

        /// <summary>
        /// показать дату и время в формате для сайта в московском часовом поясе
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToLocalMoscowVacancyDateString(this DateTime source)
        {
            var mscSource = source.AddHours(3);
            return $"{mscSource.Day.ToString("00")}-{mscSource.Month.ToString("00")}-{mscSource.Year.ToString("0000")}";
        }

        /// <summary>
        /// добавить минуты к дате с учётом выходных дней.
        /// </summary>
        /// <param name="minDateTime"></param>
        /// <param name="addingMinutes"></param>
        /// <param name="holidays"></param>
        /// <returns></returns>
        public static DateTime AddMinutesIncludingHolidays(this DateTime minDateTime, double addingMinutes, IEnumerable<DateTime> holidays)
        {
            var maxDateTime = minDateTime.AddMinutes(addingMinutes);
            var orderedHolidays = holidays.Where(c=> c >= minDateTime.Date).OrderBy(c=>c).ToList();
            foreach(var holiday in orderedHolidays)
            {
                if (holiday >= minDateTime.Date && holiday <= maxDateTime)
                {
                    maxDateTime = maxDateTime.AddMinutes(1440);
                }
                if(holiday > maxDateTime)
                    break;
            }

            return maxDateTime;
        }


    }
}
