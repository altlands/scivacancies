using System;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.WebApp.Engine
{
    public class SortFilterHelper
    {
        /// <summary>
        /// получить текстовое название свойства класса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortField"></param>
        /// <returns></returns>
        public string GetSortField<T>(string sortField)
            where T : BaseEntity
        {
            if (typeof(T) == typeof(VacancyApplication))
            {
                if (sortField == ConstTerms.OrderByFieldDate || sortField == ConstTerms.OrderByFieldCreationDate)
                    return nameof(VacancyApplication.creation_date);

                if (sortField == ConstTerms.OrderByFieldApplyDate)
                    return nameof(VacancyApplication.apply_date);

                if (sortField == ConstTerms.OrderByFieldFullName)
                    return nameof(VacancyApplication.researcher_fullname);
            }


            if (typeof(T) == typeof(Vacancy))
            {
                if (sortField == ConstTerms.OrderByFieldDate || sortField == ConstTerms.OrderByFieldCreationDate)
                    return nameof(Vacancy.creation_date);

                if (sortField == ConstTerms.OrderByFieldPublishDate)
                    return nameof(Vacancy.publish_date);

                if (sortField == ConstTerms.OrderByFieldVacancyStatus)
                    return nameof(Vacancy.status);

                if (sortField == ConstTerms.OrderByFieldClosedDate)
                    return nameof(Vacancy.update_date);
            }

            if (typeof(T) == typeof(OrganizationNotification))
            {
                if (sortField == ConstTerms.OrderByFieldCreationDate)
                    return nameof(OrganizationNotification.creation_date);
            }

            if (typeof(T) == typeof(ResearcherNotification))
            {
                if (sortField == ConstTerms.OrderByFieldCreationDate)
                    return nameof(ResearcherNotification.creation_date);
            }

            throw new Exception($"Подбор свойства для типа {typeof(T)} не реализован");

        }

    }
}
