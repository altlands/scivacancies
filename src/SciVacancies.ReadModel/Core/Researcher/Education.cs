using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Educations")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Education : BaseEntity
    {
        public Guid ResearcherGuid { get; set; }

        public string City { get; set; }
        public string UniversityShortName { get; set; }
        public string FacultyShortName { get; set; }
        public DateTime? GraduationYear { get; set; }

        public string Degree { get; set; }
    }
}
