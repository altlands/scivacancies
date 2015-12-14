using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("org_vacancies")]
    [PrimaryKey("guid", AutoIncrement = false)]
    [ExplicitColumns()]
    public class Vacancy : BaseEntity
    {
        /// <summary>
        /// Autoincremented id - нужен для "читаемого" отображения идентификатора, использовать только в GUI
        /// </summary>
        [ResultColumn]
        public long? read_id { get; set; }

        #region General

        /// <summary>
        /// Должность
        /// </summary>
        [Column]
        public string name { get; set; }

        /// <summary>
        /// Должность (Полное наименование)
        /// </summary>
        [Column]
        public string fullname { get; set; }

        /// <summary>
        /// Задачи
        /// </summary>
        [Column]
        public string tasks { get; set; }

        /// <summary>
        /// Тематика исследований
        /// </summary>
        [Column]
        public string researchtheme { get; set; }

        /// <summary>
        /// Населенный пункт 
        /// </summary>
        [Column]
        public string cityname { get; set; }

        /// <summary>
        /// Дополнительно
        /// </summary>
        [Column]
        public string details { get; set; }

        /// <summary>
        /// Лицо для получения дополнительных справок
        /// </summary>
        [Column]
        public string contact_name { get; set; }
        [Column]
        public string contact_email { get; set; }
        [Column]
        public string contact_phone { get; set; }
        [Column]
        public string contact_details { get; set; }

        #endregion

        #region Conditions

        /// <summary>
        /// Зарплата в месяц
        /// </summary>
        [Column]
        public int? salary_from { get; set; }
        [Column]
        public int? salary_to { get; set; }

        /// <summary>
        /// Стимулирующие выплаты
        /// </summary>
        [Column]
        public string bonuses { get; set; }

        /// <summary>
        /// Тип трудового договора
        /// </summary>
        [Column]
        public ContractType contract_type { get; set; }

        /// <summary>
        /// Срок трудового договора (для срочного договора)
        /// </summary>
        [Column]
        public decimal? contract_time { get; set; }

        /// <summary>
        /// Тип занятости
        /// </summary>
        [Column]
        public EmploymentType employment_type { get; set; }
        /// <summary>
        /// График работы
        /// </summary>
        [Column]
        public OperatingScheduleType operatingschedule_type { get; set; }

        /// <summary>
        /// Социальный пакет
        /// </summary>
        [Column]
        public bool socialpackage { get; set; }

        /// <summary>
        /// Найм жилья
        /// </summary>
        [Column]
        public bool rent { get; set; }

        /// <summary>
        /// Служебное жильё
        /// </summary>
        [Column]
        public bool officeaccomodation { get; set; }

        /// <summary>
        /// Компенсация транспорта
        /// </summary>
        [Column]
        public bool transportcompensation { get; set; }

        #endregion

        #region Dictionaries

        [Column]
        public int? positiontype_id { get; set; }

        /// <summary>
        /// Регион
        /// </summary>
        [Column]
        public int? region_id { get; set; }

        /// <summary>
        /// Отрасль науки
        /// </summary>
        [Column]
        public int? researchdirection_id { get; set; }

        #endregion

        #region Contest

        [Column]
        public Guid winner_researcher_guid { get; set; }
        [Column]
        public Guid winner_vacancyapplication_guid { get; set; }
        [Column]
        public DateTime? winner_request_date { get; set; }
        [Column]
        public DateTime? winner_response_date { get; set; }
        [Column]
        public bool? is_winner_accept { get; set; }

        [Column]
        public Guid pretender_researcher_guid { get; set; }
        [Column]
        public Guid pretender_vacancyapplication_guid { get; set; }
        [Column]
        public DateTime? pretender_request_date { get; set; }
        [Column]
        public DateTime? pretender_response_date { get; set; }
        [Column]
        public bool? is_pretender_accept { get; set; }

        [Column]
        public DateTime? publish_date { get; set; }

        [Column]
        public DateTime? committee_start_date { get; set; }
        [Column]
        public DateTime? committee_end_date { get; set; }
        [Column]
        public string prolonging_reason { get; set; }

        [Column]
        public DateTime? committee_date { get; set; }
        [Column]
        public DateTime? awaiting_date { get; set; }
        [Column]
        public DateTime? announcement_date { get; set; }

        /// <summary>
        /// обонсование отмены заявки
        /// </summary>
        [Column]
        public string cancel_reason { get; set; }

        /// <summary>
        /// обоснование закрытия заявки (одно для победителя и претенднта) - аналог протокола комиссии
        /// </summary>
        [Column]
        public string committee_resolution { get; set; }

        [Ignore]
        public List<VacancyCriteria> criterias { get; set; }

        /// <summary>
        /// Дополнительные критерии
        /// </summary>
        [Column]
        public string custom_criterias { get; set; }


        [Ignore]
        public List<VacancyAttachment> attachments { get; set; }

        #endregion

        /// <summary>
        /// Идентификатор организации
        /// </summary>
        [Column]
        public Guid organization_guid { get; set; }

        [Column]
        public VacancyStatus status { get; set; }

        [Column]
        public DateTime creation_date { get; set; }

        [Column]
        public DateTime? update_date { get; set; }
    }
}
