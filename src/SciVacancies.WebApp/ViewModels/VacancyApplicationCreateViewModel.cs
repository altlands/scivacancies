using System;
using System.Collections.Generic;
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

        public string Email { get; set; }
        public string ExtraEmail { get; set; }

        public string Phone { get; set; }
        public string ExtraPhone { get; set; }

        public List<EducationEditViewModel> Educations { get; set; }
        public List<PublicationEditViewModel> Publications { get; set; }

        public string ResearchActivity { get; set; }
        public string TeachingActivity { get; set; }
        public string OtherActivity { get; set; }

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

        public string CoveringLetter { get; set; }

        public List<IFormFile> Attachments { get; set; }
    }
}