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
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Необходимо заполнить Фамилию")]
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string SecondName { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить Имя на английском")]
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string FirstNameEng { get; set; }
        [Required(ErrorMessage = "Необходимо заполнить Фамилию на английском")]
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string SecondNameEng { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string PatronymicEng { get; set; }

        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string PreviousSecondName { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string PreviousSecondNameEng { get; set; }

        //public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Требуется выбрать год рождения")]
        public int BirthYear { get; set; }


        [Required(ErrorMessage = "Укажите E-mail")]
        [EmailAddress(ErrorMessage = "Поле E-mail содержит не допустимый адрес электронной почты.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите номер телефона")]
        [Phone(ErrorMessage = "Поле Телефон содержит не допустимый номер телефона.")]
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string Phone { get; set; }
        [Phone(ErrorMessage = "Поле Телефон содержит не допустимый номер телефона.")]
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string ExtraPhone { get; set; }

        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string ScienceDegree { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string ScienceRank { get; set; }
        public List<RewardEditViewModel> Rewards { get; set; } = new List<RewardEditViewModel>();
        public List<MembershipEditViewModel> Memberships { get; set; } = new List<MembershipEditViewModel>();
        public List<ConferenceEditViewModel> Conferences { get; set; } = new List<ConferenceEditViewModel>();
        public List<EducationEditViewModel> Educations { get; set; } = new List<EducationEditViewModel>();
        public List<PublicationEditViewModel> Publications { get; set; } = new List<PublicationEditViewModel>();
        public List<InterestEditViewModel> Interests { get; set; } = new List<InterestEditViewModel>();
        public List<ActivityEditViewModel> ResearchActivity { get; set; } = new List<ActivityEditViewModel>();
        public List<ActivityEditViewModel> TeachingActivity { get; set; } = new List<ActivityEditViewModel>();
        public List<ActivityEditViewModel> OtherActivity { get; set; } = new List<ActivityEditViewModel>();

        public IList<UserLoginInfo> Logins { get; set; }

        public bool IsScienceMapUser
        {
            get { return Logins != null && Logins.Any(c => c.LoginProvider == ConstTerms.LoginProviderScienceMap); }
        }

    }
}