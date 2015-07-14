using System;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("res_vacancyapplication_attachments")]
    [PrimaryKey("guid", AutoIncrement = false)]
    public class VacancyApplicationAttachment : BaseEntity
    {
        /// <summary>
        /// Наименование файла
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Размер
        /// </summary>
        public long size { get; set; }

        /// <summary>
        /// Расширение
        /// </summary>
        public string extension { get; set; }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// Guid заявки
        /// </summary>
        public Guid vacancyapplication_guid { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime upload_date { get; set; }
    }
}
