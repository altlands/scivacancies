using System;

namespace SciVacancies.WebApp
{
    public static class DatetimeExtensions
    {
        /// <summary>
        /// показать дату в формате для сайта
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToLocalVacancyDateString(this DateTime source)
        {
            return $"{source.Day.ToString("00")}-{source.Month.ToString("00")}-{source.Year.ToString("0000")}";
        }
        /// <summary>
        /// показать дату и время в формате для сайта
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToLocalVacancyDateTimeString(this DateTime source)
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


    }
}
