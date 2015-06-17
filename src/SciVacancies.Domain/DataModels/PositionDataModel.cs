using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.DataModels
{
    public class PositionDataModel
    {
        /// <summary>
        /// Должность
        /// </summary>
        public string Name { get;set;}


        /// <summary>
        /// Должность (Полное наименование)
        /// </summary>
        public string FullName { get; set; }


        /// <summary>
        /// Отрасль науки
        /// </summary>
        public string FieldOfScience { get; set; }

        /// <summary>
        /// Тематика исследований
        /// </summary>
        public string ResearchTheme { get; set; }

        /// <summary>
        /// Задачи
        /// </summary>
        public string Tasks { get; set; }

        /// <summary>
        /// Критерии оценки
        /// </summary>
        public string Criteria { get; set; }
    }
}
