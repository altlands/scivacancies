using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;

namespace SciVacancies.Domain.DataModels
{
    [Obsolete("Positions will be removed from workflow")]
    public class PositionDataModel
    {
        //TODO - что делать с критериями?
        ///// <summary>
        ///// Критерии оценки где int - идентификатор критерия, 
        ///// </summary>
        //public List<KeyValuePair<int, decimal>> Criterias { get; set; } //<CriteriaId, Amount>

        #region General

        /// <summary>
        /// Наименование должности кратко
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Наименование должности полностью
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Задачи
        /// </summary>
        public string Tasks { get; set; }

        /// <summary>
        /// Тематика исследований
        /// </summary>
        public string ResearchTheme { get; set; }

        /// <summary>
        /// Населенный пункт 
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Дополнительные условия
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Полное имя лица для получениях справок
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Почта для получения справок
        /// </summary>
        public string ContactEmail { get; set; }

        /// <summary>
        /// Телефон для справок
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// Дополнительная контактная информация
        /// </summary>
        public string ContactDetails { get; set; }

        #endregion

        #region Conditions

        /// <summary>
        /// Зарплата в месяц от
        /// </summary>
        public int SalaryFrom { get; set; }

        /// <summary>
        /// Зарплата в месяц до
        /// </summary>
        public int SalaryTo { get; set; }

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
        /// Тип занятости
        /// </summary>
        public EmploymentType EmploymentType { get; set; }

        /// <summary>
        /// График работы
        /// </summary>
        public OperatingScheduleType OperatingScheduleType { get; set; }

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

        #endregion

        #region Dictionaries

        /// <summary>
        /// Наименование позиции
        /// </summary>
        public string PositionType { get; set; }

        /// <summary>
        /// Идентификатор позиции
        /// </summary>
        public int PositionTypeId { get; set; }

        /// <summary>
        /// Название региона
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Идентфикатор региона
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// Наименование отрасли науки
        /// </summary>
        public string ResearchDirection { get; set; }

        /// <summary>
        /// Идентификатор отрасли науки
        /// </summary>
        public int ResearchDirectionId { get; set; }

        #endregion



        //TODO - привести в порядок и соответствие модели
        [Obsolete("No need to use. Will be removed.")]
        public string OrganizationName { get; set; }
        [Obsolete("You should use ResearchDirection")]
        public int ResearchThemeId { get; set; }

        /// <summary>
        /// Guid должности из справочника
        /// </summary>
        [Obsolete("Use Id")]
        public Guid PositionTypeGuid { get; set; }


        /// <summary>
        /// Критерии оценки 
        /// </summary>
        public KeyValuePair<int, int> Criteria { get; set; } //<CriteriaId, Amount>

        [Obsolete("Position status won't be in dataModel")]
        public PositionStatus Status { get; set; }
    }
}
