using System;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacancyApplicationCreateViewModel: PageViewModelBase
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

        public string ResearchActivity { get; set; }
        public string TeachingActivity { get; set; }
        public string OtherActivity { get; set; }

        public string ScienceDegree { get; set; }
        public string AcademicStatus { get; set; }
        public string Rewards { get; set; }
        public string Memberships { get; set; }
        public string Conferences { get; set; }

    }
}