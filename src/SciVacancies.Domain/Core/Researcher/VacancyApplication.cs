using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;

using System;

namespace SciVacancies.Domain.Core
{
    public class VacancyApplication
    {
        /// <summary>
        /// Guid заявки
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Guid вакансии, на которую подаётся заявка
        /// </summary>
        public Guid VacancyGuid { get; set; }

        /// <summary>
        /// Вся информация о заявке
        /// </summary>
        public VacancyApplicationDataModel Data { get; set; }

        /// <summary>
        /// Статус заявки
        /// </summary>
        public VacancyApplicationStatus Status { get; set; }
    }
}
