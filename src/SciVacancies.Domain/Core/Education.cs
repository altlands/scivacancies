using System;

namespace SciVacancies.Domain.Core
{
    public class Education
    {
        /// <summary>
        /// Guid записи об образовании
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Город получения образования
        /// </summary>
        public string City { get; set; }
        
        /// <summary>
        /// Аббревиатура учебного заведения
        /// </summary>
        public string UniversityShortName { get; set;}

        /// <summary>
        /// Аббревиатура факультета
        /// </summary>
        public string FacultyShortName { get; set; }

        /// <summary>
        /// Год окончания
        /// </summary>
        public DateTime? GraduationYear { get; set; }

        /// <summary>
        /// Академическая степень (бакалавр, магистр и т.д.)
        /// </summary>
        public string Degree { get; set; }
    }
}
