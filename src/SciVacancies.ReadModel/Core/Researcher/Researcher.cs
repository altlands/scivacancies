using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("res_researchers")]
    [PrimaryKey("guid", AutoIncrement = false)]
    public class Researcher : BaseEntity
    {
        #region General

        public string firstname { get; set; }
        public string firstname_eng { get; set; }

        public string secondname { get; set; }
        public string secondname_eng { get; set; }

        public string patronymic { get; set; }
        public string patronymic_eng { get; set; }

        public string previous_secondname { get; set; }
        public string previous_secondname_eng { get; set; }

        public DateTime birthdate { get; set; }

        public string email { get; set; }
        [Obsolete("неопределено назначение этого свойства. планируется его удаление")]
        public string extraemail { get; set; }

        public string phone { get; set; }
        public string extraphone { get; set; }

        public string nationality { get; set; }

        public string research_activity { get; set; }
        public string teaching_activity { get; set; }
        public string other_activity { get; set; }

        public string science_degree { get; set; }
        public string science_rank { get; set; }
        public string rewards { get; set; }
        public string memberships { get; set; }
        public string conferences { get; set; }

        /// <summary>
        /// Фотография исследователя
        /// </summary>
        public string image_name { get; set; }
        public long? image_size { get; set; }
        public string image_extension { get; set; }
        public string image_url { get; set; }

        /// <summary>
        /// научные инетересы
        /// </summary>
        public string interests { get; set; }

        /// <summary>
        /// Индивидуальный номер учёного
        /// </summary>
        public int ext_number { get; set; }
        #endregion

        [Ignore]
        public List<Education> educations { get; set; }
        [Ignore]
        public List<Publication> publications { get; set; }
        [Ignore]
        public List<Vacancy> favoritevacancies { get; set; }

        public ResearcherStatus status { get; set; }

        public DateTime creation_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}
