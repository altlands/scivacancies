using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class Attachment
    {
        /// <summary>
        /// Guid прикреплённого файла
        /// </summary>
        public Guid AttachmentGuid { get; set; }

        /// <summary>
        /// Наименование файла
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public double  Size { get; set; }

        /// <summary>
        /// Расширение файла
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string Url { get; set; }
    }
}
