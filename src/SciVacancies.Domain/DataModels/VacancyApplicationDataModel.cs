using SciVacancies.Domain.Core;
using System.Collections.Generic;

namespace SciVacancies.Domain.DataModels
{
    public class VacancyApplicationDataModel
    {
        public VacancyApplicationDataModel()
        {
            this.Educations = new List<Education>();
            this.Publications = new List<Publication>();
            this.Attachments = new List<Attachment>();
        }

        /// <summary>
        /// Полное имя исследователя
        /// </summary>
        public string ResearcherFullName { get; set; }
        public string PositionName { get; set; }

        public string Email { get; set; }
        public string ExtraEmail { get; set; }

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

        public List<Education> Educations { get; set; }
        public List<Publication> Publications { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
