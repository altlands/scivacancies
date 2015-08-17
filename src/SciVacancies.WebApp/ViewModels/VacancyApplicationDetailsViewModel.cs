using System;
using System.Collections.Generic;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacancyApplicationDetailsViewModel: PageViewModelBase
    {

        public Guid VacancyGuid { get; set; }
        public VacancyDetailsViewModel Vacancy { get; set; }
        public Guid ResearcherGuid { get; set; }
        public ResearcherDetailsViewModel Researcher { get; set; }

        public int PositionTypeId { get; set; }
        public string PositionTypeName{ get; set; }

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


        public VacancyApplicationStatus Status { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? SentDate { get; set; }
        /// <summary>
        /// Прикрепленные файлы
        /// </summary>
        public IEnumerable<VacancyApplicationAttachment> Attachments { get; set; }

        public string CoveringLetter { get; set; }

        /// <summary>
        /// Образование
        /// </summary>
        public List<Education> Educations { get; set; }
        /// <summary>
        /// Публикации
        /// </summary>
        public List<Publication> Publications { get; set; }
    }
}
