
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Events
{
    public class OrganizationDataModel
    {
        /// <summary>
        /// Полное наименование
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// Полное наименование (на английском языке)
        /// </summary>
        public string NameEng { get; set; }

        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Сокращенное наименование (на английском языке)
        /// </summary>
        public string ShortNameEng { get; set; }

        /// <summary>
        /// Населенный пункт
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Почтовый адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Веб-сайт
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public string INN { get; set; }
    }
}
