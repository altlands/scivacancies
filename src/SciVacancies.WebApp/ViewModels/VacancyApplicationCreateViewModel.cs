using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacancyApplicationCreateViewModel : PageViewModelBase
    {

        [HiddenInput]
        public Guid ResearcherGuid { get; set; }
        [HiddenInput]
        public Guid VacancyGuid { get; set; }
        public string VacancyCode { get; set; }

        public string PositionName { get; set; }
        public string ResearcherFullName { get; set; }
        public string ResearcherFullNameEng { get; set; }

        public DateTime BirthDate { get; set; }
        public string ImageUrl { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public string ExtraPhone { get; set; }

        public List<EducationEditViewModel> Educations { get; set; } = new List<EducationEditViewModel>();
        public List<PublicationEditViewModel> Publications { get; set; } = new List<PublicationEditViewModel>();

        public string ResearchActivity { get; set; }
        public List<ActivityEditViewModel> ResearchActivityDeserialized => !string.IsNullOrWhiteSpace(ResearchActivity)
            ? JsonConvert.DeserializeObject<List<ActivityEditViewModel>>(ResearchActivity)
            : new List<ActivityEditViewModel>();

        public string TeachingActivity { get; set; }
        public List<ActivityEditViewModel> TeachingActivityDeserialized => !string.IsNullOrWhiteSpace(TeachingActivity)
            ? JsonConvert.DeserializeObject<List<ActivityEditViewModel>>(TeachingActivity)
            : new List<ActivityEditViewModel>();

        public string OtherActivity { get; set; }
        public List<ActivityEditViewModel> OtherActivityDeserialized => !string.IsNullOrWhiteSpace(OtherActivity)
            ? JsonConvert.DeserializeObject<List<ActivityEditViewModel>>(OtherActivity)
            : new List<ActivityEditViewModel>();

        public string ScienceDegree { get; set; }
        public string ScienceRank { get; set; }
        public string Rewards { get; set; }
        public List<RewardDetailsViewModel> RewardsDeserialized => !string.IsNullOrWhiteSpace(Rewards)
            ? JsonConvert.DeserializeObject<List<RewardDetailsViewModel>>(Rewards)
            : new List<RewardDetailsViewModel>();

        public string Memberships { get; set; }
        public List<MembershipDetailsViewModel> MembershipsDeserialized => !string.IsNullOrWhiteSpace(Memberships)
            ? JsonConvert.DeserializeObject<List<MembershipDetailsViewModel>>(Memberships)
            : new List<MembershipDetailsViewModel>();

        public string Conferences { get; set; }
        public List<ConferenceEditViewModel> ConferencesDeserialized => !string.IsNullOrWhiteSpace(Conferences)
            ? JsonConvert.DeserializeObject<List<ConferenceEditViewModel>>(Conferences)
            : new List<ConferenceEditViewModel>();

        public string Interests { get; set; }
        public List<InterestEditViewModel> InterestsDeserialized => !string.IsNullOrWhiteSpace(Interests)
            ? JsonConvert.DeserializeObject<List<InterestEditViewModel>>(Interests)
            : new List<InterestEditViewModel>();

        [MaxLength(4000, ErrorMessage = "Длина строки не более 4000 символов")]
        public string CoveringLetter { get; set; }

        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();

        /// <summary>
        /// Autoincrimented field in DB - может быть null
        /// </summary>
        public long? ReadId { get; set; }

    }
}