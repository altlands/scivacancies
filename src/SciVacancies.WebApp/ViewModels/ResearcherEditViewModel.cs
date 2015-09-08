using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNet.Identity;
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
        [EmailAddress(ErrorMessage = "Поле E-mail содержит не допустимый адрес электронной почты.")]
        [Obsolete("неопределено назначение этого свойства. планируется его удаление")]
        public string ExtraEmail { get; set; }

        [Required(ErrorMessage = "Укажите номер телефона")]
        [Phone(ErrorMessage = "Поле Телефон содержит не допустимый номер телефона.")]
        public string Phone { get; set; }
        [Phone(ErrorMessage = "Поле Телефон содержит не допустимый номер телефона.")]
        public string ExtraPhone { get; set; }

        public string Nationality { get; set; }

        public string ResearchActivity { get; set; }
        public string TeachingActivity { get; set; }
        public string OtherActivity { get; set; }

        public string ScienceDegree { get; set; }
        public string ScienceRank { get; set; }
        public List<RewardEditViewModel> Rewards { get; set; }
        public List<MembershipEditViewModel> Memberships { get; set; }
        public string Conferences { get; set; }

        public List<EducationEditViewModel> Educations { get; set; }
        public List<PublicationEditViewModel> Publications { get; set; }
        public List<InterestEditViewModel> Interests { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public bool IsScienceMapUser
        {
            get { return Logins != null && Logins.Any(c => c.LoginProvider == ConstTerms.LoginProviderScienceMap); }
        }

    }
}