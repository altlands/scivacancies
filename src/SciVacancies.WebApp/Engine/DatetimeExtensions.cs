using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.WebApp
{
    public static class DatetimeExtensions
    {
        public static string ToLocalVacancyString(this DateTime source)
        {
            return $"{source.Day}-{source.Month}-{source.Year}, {source.Minute}:{source.Minute}";
        }
    }
}
