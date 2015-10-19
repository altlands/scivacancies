using System;
using SciVacancies.Domain.Core;
using System.Collections.Generic;

namespace SciVacancies.Domain.DataModels
{
    public class VacancyApplicationDataModel
    {
        /// <summary>
        /// Полное имя исследователя
        /// </summary>
        public string ResearcherFullName { get; set; }
        public string ResearcherFullNameEng { get; set; }
        public string PositionName { get; set; }

        public DateTime BirthDate { get; set; }
        public string ImageUrl { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public string ExtraPhone { get; set; }

        public string ResearchActivity { get; set; }
        public string TeachingActivity { get; set; }
        public string OtherActivity { get; set; }

        public string ScienceDegree { get; set; }
        public string ScienceRank { get; set; }
        public string Rewards { get; set; }
        public string Memberships { get; set; }
        public string Conferences { get; set; }
        public string Interests { get; set; }

        public string CoveringLetter { get; set; }

        public List<Education> Educations { get; set; } = new List<Education>();
        public List<Publication> Publications { get; set; } = new List<Publication>();
        public List<VacancyApplicationAttachment> Attachments { get; set; } = new List<VacancyApplicationAttachment>();
    }
}
