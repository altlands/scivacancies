using System;

namespace SciVacancies.Domain.Core
{
    public class Publication
    {
        /// <summary>
        /// Guid публикации
        /// </summary>
        public Guid PublicationGuid { get; set; }

        /// <summary>
        /// Название публикации
        /// </summary>
        public string Name { get; set; }
    }
}
