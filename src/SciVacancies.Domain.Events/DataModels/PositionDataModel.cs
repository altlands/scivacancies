using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Events
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
        public KeyValuePair<int,int> Criteria { get; set; } //<CriteriaId, Amount>

        /// <summary>
        /// Зарплата в месяц
        /// </summary>
        public int SalaryFrom { get; set; }
        public int SalaryTo { get; set; }
        //public Currency SalaryCurrency { get; set; }

        /// <summary>
        /// Стимулирующие выплаты
        /// </summary>
        public string Bonuses { get; set; }

        /// <summary>
        /// Дополнительно
        /// </summary>
        public string Additional { get; set; }

        /// <summary>
        /// Дополнительно
        /// </summary>
        public ContractType ContractType { get; set; }





    }
}
