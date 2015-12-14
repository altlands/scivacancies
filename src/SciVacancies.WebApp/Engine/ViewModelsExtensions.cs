using System;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp
{

    public static class ViewModelsExtensions
    {
        private static bool CheckRequiredFilled(int? positionTypeId, int? researchDirectionId, int? regionId, string tasks, int? salaryFrom, int? salaryTo, string contactName, string contactEmail, string contactPhone)
        {
            return positionTypeId.HasValue
                && researchDirectionId.HasValue
                && regionId.HasValue
                && !string.IsNullOrWhiteSpace(tasks)
                && salaryFrom.HasValue
                && salaryTo.HasValue
                && !string.IsNullOrWhiteSpace(contactName)
                && !string.IsNullOrWhiteSpace(contactEmail)
                && !string.IsNullOrWhiteSpace(contactPhone);
        }

        /// <summary>
        /// проверить заполнены ли все обязательные поля перед публикацией
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool RequiredFilled(this VacancyCreateViewModel source)
        {
            return CheckRequiredFilled(source.PositionTypeId, source.ResearchDirectionId, source.RegionId, source.Tasks, source.SalaryFrom, source.SalaryTo, source.ContactName, source.ContactEmail, source.ContactPhone);
        }

        /// <summary>
        /// проверить заполнены ли все обязательные поля перед публикацией
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool RequiredFilled(this VacancyDetailsViewModel source)
        {
            return CheckRequiredFilled(source.PositionTypeId, source.ResearchDirectionId, source.RegionId, source.Tasks, source.SalaryFrom, source.SalaryTo, source.ContactName, source.ContactEmail, source.ContactPhone);
        }

        /// <summary>
        /// проверить заполнены ли все обязательные поля перед публикацией
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool RequiredFilled(this SciVacancies.ReadModel.Core.Vacancy source)
        {
            return CheckRequiredFilled(source.positiontype_id, source.researchdirection_id, source.region_id, source.tasks, source.salary_from, source.salary_to, source.contact_name, source.contact_email, source.contact_phone);
        }


    }
}
