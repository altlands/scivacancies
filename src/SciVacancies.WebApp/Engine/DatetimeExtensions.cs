using System;

namespace SciVacancies.WebApp
{
    public static class DatetimeExtensions
    {
        public static string ToLocalVacancyDateString(this DateTime source)
        {
            return $"{source.Day.ToString("00")}-{source.Month.ToString("00")}-{source.Year.ToString("0000")}";
        }
        public static string ToLocalVacancyString(this DateTime source)
        {
            return $"{source.Day.ToString("00")}-{source.Month.ToString("00")}-{source.Year.ToString("0000")}, {source.Hour.ToString("00")}:{source.Minute.ToString("00")}";
        }


    }
}
