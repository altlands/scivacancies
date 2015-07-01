using SciVacancies.Domain.Core;
using System.Collections.Generic;

namespace SciVacancies.Domain.DataModels
{
    public class VacancyApplicationDataModel
    {
        /// <summary>
        /// Полное имя исследователя
        /// </summary>
        public string FullName { get; set; }

        public string Email { get; set; }
        public string ExtraEmail { get; set; }

        public string Phone { get; set; }
        public string ExtraPhone { get; set; }

        public string ResearchActivity { get; set; }
        public string TeachingActivity { get; set; }
        public string OtherActivity { get; set; }

        public string ScienceDegree { get; set; }
        public string AcademicStatus { get; set; }
        public string Rewards { get; set; }
        public string Memberships { get; set; }
        public string Conferences { get; set; }

        public List<Education> Educations { get; set; }
        public List<Publication> Publications { get; set; }
    }
}
