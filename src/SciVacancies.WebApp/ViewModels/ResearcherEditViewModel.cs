using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNet.Identity;
using SciVacancies.WebApp.Models;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class ResearcherEditViewModel: PageViewModelBase
    {
        public int ExtNumber { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить Имя")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Необходимо заполнить Фамилию")]
        public string SecondName { get; set; }
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить Имя на английском")]
        public string FirstNameEng { get; set; }
        [Required(ErrorMessage = "Необходимо заполнить Фамилию на английском")]
        public string SecondNameEng { get; set; }
        public string PatronymicEng { get; set; }

        public string PreviousSecondName { get; set; }
        ///// <summary>
        ///// Прежняя фамилия
        ///// </summary>
        public string PreviousSecondNameEng { get; set; }

        //public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Требуется выбрать год рождения")]
        public int BirthYear { get; set; }


        [Required(ErrorMessage = "Укажите E-mail")]
        [EmailAddress(ErrorMessage = "Поле E-mail содержит не допустимый адрес электронной почты.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите номер телефона")]
        [Phone(ErrorMessage = "Поле Телефон содержит не допустимый номер телефона.")]
        public string Phone { get; set; }
        [Phone(ErrorMessage = "Поле Телефон содержит не допустимый номер телефона.")]
        public string ExtraPhone { get; set; }

        public string Nationality { get; set; }

        public string ScienceDegree { get; set; }
        public string ScienceRank { get; set; }
        public List<RewardEditViewModel> Rewards { get; set; } = new CheckableList<RewardEditViewModel>();
        public List<MembershipEditViewModel> Memberships { get; set; } = new CheckableList<MembershipEditViewModel>();
        public List<ConferenceEditViewModel> Conferences { get; set; } = new CheckableList<ConferenceEditViewModel>();
        public List<EducationEditViewModel> Educations { get; set; } = new CheckableList<EducationEditViewModel>();
        public List<PublicationEditViewModel> Publications { get; set; } = new CheckableList<PublicationEditViewModel>();
        public List<InterestEditViewModel> Interests { get; set; } = new CheckableList<InterestEditViewModel>();
        public List<ActivityEditViewModel> ResearchActivity { get; set; } = new CheckableList<ActivityEditViewModel>();
        public List<ActivityEditViewModel> TeachingActivity { get; set; } = new CheckableList<ActivityEditViewModel>();
        public List<ActivityEditViewModel> OtherActivity { get; set; } = new CheckableList<ActivityEditViewModel>();

        public IList<UserLoginInfo> Logins { get; set; }

        public bool IsScienceMapUser
        {
            get { return Logins != null && Logins.Any(c => c.LoginProvider == ConstTerms.LoginProviderScienceMap); }
        }

    }
}