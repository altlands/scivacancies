using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("organizations")]
    [PrimaryKey("guid", AutoIncrement = false)]
    public class Organization : BaseEntity
    {
        /// <summary>
        /// Полное наименование
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public string shortname { get; set; }

        /// <summary>
        /// Почтовый адрес
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public string inn { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public string ogrn { get; set; }

        /// <summary>
        /// Руководитель
        /// </summary>
        public string head_firstname { get; set; }
        public string head_secondname { get; set; }
        public string head_patronymic { get; set; }

        /// <summary>
        /// ФОИВ
        /// </summary>
        public int foiv_id { get; set; }

        /// <summary>
        /// Организационно-правовая форма организации
        /// </summary>
        public int orgform_id { get; set; }

        public List<ResearchDirection> researchdirections { get; set; }

        public OrganizationStatus status { get; set; }

        public DateTime creation_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}