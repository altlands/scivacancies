using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;

using System;

namespace SciVacancies.Domain.Core
{
    public class Vacancy
    {
        /// <summary>
        /// Guid вакансии
        /// </summary>
        public Guid VacancyGuid { get; set; }

        /// <summary>
        /// Guid родительской позиции
        /// </summary>
        [Obsolete("Will be removed")]
        public Guid PositionGuid { get; set; }

        /// <summary>
        /// Вся информация о вакансии
        /// </summary>
        public VacancyDataModel Data { get; set; }

        /// <summary>
        /// Статус вакансии
        /// </summary>
        public VacancyStatus Status { get; set; }
    }
}
