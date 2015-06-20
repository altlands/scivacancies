using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Attachments")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Attachment : BaseEntity
    {
        /// <summary>
        /// Guid заявки
        /// </summary>
        public Guid VacancyApplicationGuid { get; set; }
        /// <summary>
        /// Наименование файла
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Размер
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// Расширение
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreationdDate { get; set; }
    }
}
