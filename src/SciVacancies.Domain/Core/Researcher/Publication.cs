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
        public string Title { get; set; }
    }
}
