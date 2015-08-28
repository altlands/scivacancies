using System;

namespace SciVacancies.Domain.Core
{
    public class Publication
    {
        /// <summary>
        /// Guid публикации
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Название публикации
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// авторы
        /// </summary>
        public string Authors { get; set; }

        public string ExtId { get; set; }

        public string Type { get; set; }

        public DateTime? Updated { get; set; }
    }
}
