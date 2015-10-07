using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("res_vacancyapplications")]
    [PrimaryKey("guid", AutoIncrement = false)]
    [ExplicitColumns()]
    public class VacancyApplication : BaseEntity
    {
        /// <summary>
        /// Autoincremented id - нужен для "читаемого" отображения идентификатора, использовать только в GUI
        /// </summary>
        [ResultColumn]
        public long? read_id { get; set; }

        [Column]
        public string researcher_fullname { get; set; }
        [Column]
        public string position_name { get; set; }

        [Column]
        public string email { get; set; }
        [Obsolete("неопределено назначение этого свойства. планируется его удаление")]
        [Column]
        public string extraemail { get; set; }
        [Column]
        public string phone { get; set; }
        [Column]
        public string extraphone { get; set; }

        [Column]
        public string research_activity { get; set; }
        [Column]
        public string teaching_activity { get; set; }
        [Column]
        public string other_activity { get; set; }

        [Column]
        public string science_degree { get; set; }
        [Column]
        public string science_rank { get; set; }
        [Column]
        public string rewards { get; set; }
        [Column]
        public string memberships { get; set; }
        [Column]
        public string conferences { get; set; }

        [Column]
        public string covering_letter { get; set; }

        [Column]
        public string educations { get; set; }
        [Column]
        public string publications { get; set; }

        [Column]
        public Guid vacancy_guid { get; set; }
        [Column]
        public Guid researcher_guid { get; set; }

        [Ignore]
        public List<VacancyApplicationAttachment> attachments { get; set; }

        [Column]
        public VacancyApplicationStatus status { get; set; }

        [Column]
        public DateTime creation_date { get; set; }
        [Column]
        public DateTime? update_date { get; set; }

        /// <summary>
        /// Дата перевода заявки из статуса "черновик" в статус "отправлена"
        /// </summary>
        [Column]
        public DateTime? apply_date { get; set; }
    }
}
