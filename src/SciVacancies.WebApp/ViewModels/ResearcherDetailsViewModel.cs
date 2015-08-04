using System;
using System.Collections.Generic;
using NPoco;
using SciVacancies.WebApp.ViewModels.Base;
using SciVacancies.Domain.Enums;

namespace SciVacancies.WebApp.ViewModels
{
    public class ResearcherDetailsViewModel : PageViewModelBase
    {
        public ResearcherDetailsViewModel()
        {
            Title = "Личная карточка пользователя";
        }

        [Ignore]
        public string Login { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }

        public string FirstNameEng { get; set; }
        public string SecondNameEng { get; set; }
        public string PatronymicEng { get; set; }

        public string PreviousSecondName { get; set; }
        public string PreviousSecondNameEng { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }
        public string ExtraEmail { get; set; }

        public string Phone { get; set; }
        public string ExtraPhone { get; set; }

        public string Nationality { get; set; }

        public string ResearchActivity { get; set; }
        public string TeachingActivity { get; set; }
        public string OtherActivity { get; set; }

        public string ScienceDegree { get; set; }
        public string ScienceRank { get; set; }
        public string Rewards { get; set; }
        public string Memberships { get; set; }
        public string Conferences { get; set; }

        public string ImageName { get; set; }
        public long? ImageSize { get; set; }
        public string ImageExtension { get; set; }
        public string ImageUrl { get; set; }

        public List<EducationEditViewModel> Educations { get; set; }
        public List<PublicationEditViewModel> Publications { get; set; }

        public ResearcherStatus Status { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string FullName => $"{SecondName} {FirstName} {Patronymic}";
        public string FullNameEng => $"{FirstNameEng} {PatronymicEng} {SecondNameEng}";
    }
}
