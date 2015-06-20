using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class Attachment
    {
        /// <summary>
        /// Guid файла
        /// </summary>
        public Guid AttachmentGuid { get; set; }
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
    }
}
