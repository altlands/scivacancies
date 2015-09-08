using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    [MetadataType(typeof(VacancyApplicationSetWinnerViewModelMetadata))]
    public class VacancyApplicationSetWinnerViewModel : PageViewModelBase
    {
        internal sealed class VacancyApplicationSetWinnerViewModelMetadata
        {
            [Required]
            [HiddenInput]
            public Guid Guid { get; set; }
        }

        public List<IFormFile> Attachments { get; set; }

        [Required]
        public Guid VacancyGuid { get; set; }
        public VacancyDetailsViewModel Vacancy { get; set; }
        [Required]
        public Guid ResearcherGuid { get; set; }
        public ResearcherDetailsViewModel Researcher { get; set; }
        /// <summary>
        /// Образование
        /// </summary>
        public List<Education> Educations { get; set; }
        /// <summary>
        /// Публикации
        /// </summary>
        public List<Publication> Publications { get; set; }



        public int PositionTypeId { get; set; }
        public string PositionTypeName { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        [Obsolete("неопределено назначение этого свойства. планируется его удаление")]
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

        public string CoveringLetter { get; set; }

        public VacancyApplicationStatus Status { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? SentDate { get; set; }

        //public bool IsWinner { get; set; }
        [Required]
        public string Reason { get; set; }

        /// <summary>
        /// кого выбираем - победителя или претендента
        /// </summary>
        public bool WinnerIsSetting { get; set; }
    }
}
