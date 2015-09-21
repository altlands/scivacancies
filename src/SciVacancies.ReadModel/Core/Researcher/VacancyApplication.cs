using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("res_vacancyapplications")]
    [PrimaryKey("guid", AutoIncrement = false)]
    public class VacancyApplication : BaseEntity
    {
        /// <summary>
        /// Autoincremented id - нужен для "читаемого" отображения идентификатора, использовать только в GUI
        /// </summary>
        public long read_id { get; set; }

        public string researcher_fullname { get; set; }
        public string position_name { get; set; }

        public string email { get; set; }
        [Obsolete("неопределено назначение этого свойства. планируется его удаление")]
        public string extraemail { get; set; }
        public string phone { get; set; }
        public string extraphone { get; set; }

        public string research_activity { get; set; }
        public string teaching_activity { get; set; }
        public string other_activity { get; set; }

        public string science_degree { get; set; }
        public string science_rank { get; set; }
        public string rewards { get; set; }
        public string memberships { get; set; }
        public string conferences { get; set; }

        public string covering_letter { get; set; }

        public string educations { get; set; }
        public string publications { get; set; }

        public Guid vacancy_guid { get; set; }
        public Guid researcher_guid { get; set; }

        [Ignore]
        public List<VacancyApplicationAttachment> attachments { get; set; }

        public VacancyApplicationStatus status { get; set; }

        public DateTime creation_date { get; set; }
        public DateTime? update_date { get; set; }

        /// <summary>
        /// Дата перевода заявки из статуса "черновик" в статус "отправлена"
        /// </summary>
        public DateTime? apply_date { get; set; }
    }
}
