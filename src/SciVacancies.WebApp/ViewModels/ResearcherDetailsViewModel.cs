using System;
using System.Collections.Generic;
using NPoco;
using SciVacancies.WebApp.ViewModels.Base;
using SciVacancies.Domain.Enums;
using SciVacancies.WebApp.Engine;

namespace SciVacancies.WebApp.ViewModels
{
    public class ResearcherDetailsViewModel : PageViewModelBase
    {

        public ResearcherDetailsViewModel()
        {
            Title = "Личная карточка пользователя";
        }

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

        public string Phone { get; set; }
        public string ExtraPhone { get; set;}

        public List<ActivityEditViewModel> ResearchActivity { get; set; } = new List<ActivityEditViewModel>();
        public List<ActivityEditViewModel> TeachingActivity { get; set; } = new List<ActivityEditViewModel>();
        public List<ActivityEditViewModel> OtherActivity { get; set; } = new List<ActivityEditViewModel>();

        public string ScienceDegree { get; set; }
        public string ScienceRank { get; set; }
        public List<RewardDetailsViewModel> Rewards { get; set; } = new List<RewardDetailsViewModel>();
        public List<MembershipDetailsViewModel> Memberships { get; set; } = new List<MembershipDetailsViewModel>();
        public List<InterestDetailsViewModel> Interests { get; set; } = new List<InterestDetailsViewModel>();
        public List<ConferenceEditViewModel> Conferences { get; set; } = new List<ConferenceEditViewModel>();

        public string ImageName { get; set; }
        public long? ImageSize { get; set; }
        public string ImageExtension { get; set; }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl ?? string.Empty; }
            set { _imageUrl = value; }
        }
        /// <summary>
        /// Индивидуальный номер учёного
        /// </summary>
        public int ExtNumber { get; set; }

        public List<EducationEditViewModel> Educations { get; set; } = new List<EducationEditViewModel>();
        public List<PublicationEditViewModel> Publications { get; set; } = new List<PublicationEditViewModel>();

        public ResearcherStatus Status { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string FullName => $"{SecondName} {FirstName} {Patronymic}";
        public string FullNameEng => $"{FirstNameEng} {PatronymicEng} {SecondNameEng}";

    }
}
