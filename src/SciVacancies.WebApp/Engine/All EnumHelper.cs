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
                case VacancyStatus.Cancelled:
                    return "failed";
                case VacancyStatus.Closed:
                case VacancyStatus.Published:
                case VacancyStatus.InCommittee:
                    return "executed";
                case VacancyStatus.AppliesAcceptance:
                    return "work";
                default: return null;
            }
        }

        public static string GetHtmlClass(this PositionStatus value)
        {
            switch (value)
            {
                case PositionStatus.Removed:
                case PositionStatus.InProcess:
                    return "failed"; 
                case PositionStatus.Published:
                    return "executed";
                default:
                    return null;
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
