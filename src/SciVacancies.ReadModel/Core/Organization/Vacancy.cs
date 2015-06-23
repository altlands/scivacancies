using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;
using Nest;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Vacancies")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Vacancy : BaseEntity
    {
        /// <summary>
        /// Идентификатор организации
        /// </summary>
        public Guid OrganizationGuid { get; set; }
        public Guid PositionGuid { get; set; }
        public Guid PositionTypeGuid { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public Guid WinnerGuid { get; set; }
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public Guid PretenderGuid { get; set; }

        public string OrganizationName { get; set; }
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
        public string ResearchDirection { get; set; }
        public int ResearchDirectionId { get; set; }
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
        [Ignore]
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
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
        public string Region { get; set; }
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

        public VacancyStatus Status { get; set; }
        /// <summary>
        /// Количество добавивших вакансию в избранное
        /// </summary>
        public int FollowersCounter { get; set; }

        /// <summary>
        /// Дата начала публикации
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// Дата начала приёма заявок на вакансию
        /// </summary>
        public DateTime DateStartAcceptance { get; set; }
        /// <summary>
        /// Дата окончания приёма заявок на вакансию
        /// </summary>
        public DateTime DateFinishAcceptance { get; set; }
        /// <summary>
        /// Дата окончания публикации
        /// </summary>
        public DateTime DateFinish { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
