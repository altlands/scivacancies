using System;
using System.ComponentModel;
using System.Reflection;
using SciVacancies.Domain.Enums;

namespace SciVacancies.WebApp
{

    public static class EnumExtenions
    {
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);

            if (name == null) return null;

            var field = type.GetField(name);

            if (field == null) return null;

            var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attr?.Description;
        }

        public static string GetHtmlClass(this VacancyStatus value)
        {
            switch (value)
            {
                case VacancyStatus.InProcess:
                    return "draft";
                case VacancyStatus.Published:
                case VacancyStatus.InCommittee:
                    return "work";
                case VacancyStatus.Closed:
                    return "executed";
                case VacancyStatus.Cancelled:
                    return "failed";
                case VacancyStatus.OfferAccepted:
                    return "work";
                case VacancyStatus.OfferRejected:
                case VacancyStatus.Removed:
                default:  return "failed";
            }
        }

        public static string GetHtmlClass(this VacancyApplicationStatus value)
        {
            switch (value)
            {
                case VacancyApplicationStatus.Cancelled:
                case VacancyApplicationStatus.Removed:
                    return "failed";
                case VacancyApplicationStatus.InProcess:
                case VacancyApplicationStatus.Applied:
                case VacancyApplicationStatus.Lost:
                    return "work"; 
                case VacancyApplicationStatus.Won:
                case VacancyApplicationStatus.Pretended:
                    return "executed";
                default:
                    return null;
            }
        }
    }
}
