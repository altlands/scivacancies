using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Positions")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Position : BaseEntity
    {
        /// <summary>
        /// Идентификатор организации
        /// </summary>
        public Guid OrganizationGuid { get; set; }

        /// <summary>
        /// Guid должности из справочника
        /// </summary>
        public Guid PositionTypeGuid { get; set; }
        /// <summary>
        /// Должность
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Должность (Полное наименование)
        /// </summary>
        public string FullName { get; set; }


        /// <summary>
        /// Отрасль науки
        /// </summary>
        public string FieldOfScience { get; set; }
        public int FieldOfScienceId { get; set; }

        /// <summary>
        /// Тематика исследований
        /// </summary>
        public string ResearchTheme { get; set; }
        public int ResearchThemeId { get; set; }

        /// <summary>
        /// Задачи
        /// </summary>
        public string Tasks { get; set; }

        /// <summary>
        /// Критерии оценки 
        /// </summary>
        public KeyValuePair<int, int> Criteria { get; set; } //<CriteriaId, Amount>

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
        /// Тип трудового договора
        /// </summary>
        public ContractType ContractType { get; set; }

        /// <summary>
        /// Срок трудового договора (для срочного договора)
        /// </summary>
        public decimal ContractTime { get; set; }

        /// <summary>
        /// Социальный пакет
        /// </summary>
        public bool SocialPackage { get; set; }

        /// <summary>
        /// Найм жилья
        /// </summary>
        public bool Rent { get; set; }

        /// <summary>
        /// Служебное жильё
        /// </summary>
        public bool OfficeAccomodation { get; set; }

        /// <summary>
        /// Компенсация транспорта
        /// </summary>
        public bool TransportCompensation { get; set; }

        /// <summary>
        /// Регион
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// Населенный пункт 
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Дополнительно
        /// </summary>
        public string Details { get; set; }


        /// <summary>
        /// Лицо для получения дополнительных справок
        /// </summary>
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactDetails { get; set; }

        public DateTime CreationdDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public PositionStatus Status { get; set; }
    }
}
