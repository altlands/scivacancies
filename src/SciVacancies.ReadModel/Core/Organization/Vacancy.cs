using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("org_vacancies")]
    [PrimaryKey("guid", AutoIncrement = false)]
    public class Vacancy : BaseEntity
    {
        #region General

        /// <summary>
        /// Должность
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Должность (Полное наименование)
        /// </summary>
        public string fullname { get; set; }

        /// <summary>
        /// Задачи
        /// </summary>
        public string tasks { get; set; }

        /// <summary>
        /// Тематика исследований
        /// </summary>
        public string researchtheme { get; set; }

        /// <summary>
        /// Населенный пункт 
        /// </summary>
        public string cityname { get; set; }

        /// <summary>
        /// Дополнительно
        /// </summary>
        public string details { get; set; }

        /// <summary>
        /// Лицо для получения дополнительных справок
        /// </summary>
        public string contact_name { get; set; }
        public string contact_email { get; set; }
        public string contact_phone { get; set; }
        public string contact_details { get; set; }

        #endregion

        #region Conditions

        /// <summary>
        /// Зарплата в месяц
        /// </summary>
        public int? salary_from { get; set; }
        public int? salary_to { get; set; }

        /// <summary>
        /// Стимулирующие выплаты
        /// </summary>
        public string bonuses { get; set; }

        /// <summary>
        /// Тип трудового договора
        /// </summary>
        public ContractType contract_type { get; set; }

        /// <summary>
        /// Срок трудового договора (для срочного договора)
        /// </summary>
        public decimal? contract_time { get; set; }

        /// <summary>
        /// Тип занятости
        /// </summary>
        public EmploymentType employment_type { get; set; }
        /// <summary>
        /// График работы
        /// </summary>
        public OperatingScheduleType operatingschedule_type { get; set; }

        /// <summary>
        /// Социальный пакет
        /// </summary>
        public bool socialpackage { get; set; }

        /// <summary>
        /// Найм жилья
        /// </summary>
        public bool rent { get; set; }

        /// <summary>
        /// Служебное жильё
        /// </summary>
        public bool officeaccomodation { get; set; }

        /// <summary>
        /// Компенсация транспорта
        /// </summary>
        public bool transportcompensation { get; set; }

        #endregion

        #region Dictionaries

        public int positiontype_id { get; set; }

        /// <summary>
        /// Регион
        /// </summary>
        public int region_id { get; set; }

        /// <summary>
        /// Отрасль науки
        /// </summary>
        public int researchdirection_id { get; set; }

        #endregion

        #region Contest

        public Guid winner_researcher_guid { get; set; }
        public Guid winner_vacancyapplication_guid { get; set; }
        public DateTime? winner_request_date { get; set; }
        public DateTime? winner_response_date { get; set; }
        public bool? is_winner_accept { get; set; }

        public Guid pretender_researcher_guid { get; set; }
        public Guid pretender_vacancyapplication_guid { get; set; }
        public DateTime? pretender_request_date { get; set; }
        public DateTime? pretender_response_date { get; set; }
        public bool? is_pretender_accept { get; set; }

        public DateTime? publish_date { get; set; }

        public DateTime? committee_start_date { get; set; }
        public DateTime? committee_end_date { get; set; }
        public string prolonging_reason { get; set; }

        public DateTime? committee_date { get; set; }
        public DateTime? awaiting_date { get; set; }
        public DateTime? announcement_date { get; set; }

        /// <summary>
        /// обонсование отмены заявки
        /// </summary>
        public string cancel_reason { get; set; }

        /// <summary>
        /// обоснование закрытия заявки (одно для победителя и претенднта) - аналог протокола комиссии
        /// </summary>
        public string committee_resolution { get; set; }

        [Ignore]
        public List<VacancyCriteria> criterias { get; set; }
        [Ignore]
        public List<VacancyAttachment> attachments { get; set; }

        #endregion

        /// <summary>
        /// Идентификатор организации
        /// </summary>
        public Guid organization_guid { get; set; }

        public VacancyStatus status { get; set; }

        public DateTime creation_date { get; set; }

        public DateTime? update_date { get; set; }
    }
}
