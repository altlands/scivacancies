using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class Education
    {
        public Guid EducationGuid { get; set; }

        public string City { get; set; }
        public string UniversityShortName { get; set;}
        public string FacultyShortName { get; set; }
        public DateTime GraduationYear { get; set; }

        public string Degree { get; set; }
    }
}
