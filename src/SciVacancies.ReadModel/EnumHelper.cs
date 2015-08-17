using System;
using System.ComponentModel;
using SciVacancies.Domain.Enums;

namespace SciVacancies
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


        public static string GetDescriptionExtended(this VacancyStatus value)
        {
            switch (value)
            {
                case VacancyStatus.InCommittee:
                    return "Ожидается выбор Победителя и Претендента";
                case VacancyStatus.OfferResponseAwaiting:
                    return "Ожидается ответ от Победителя/Претендента";
                case VacancyStatus.OfferAccepted:
                case VacancyStatus.OfferRejected:
                    return "Ождидается закрытие вакансии";
                default:
                    return "";
            }
        }
    }
}
