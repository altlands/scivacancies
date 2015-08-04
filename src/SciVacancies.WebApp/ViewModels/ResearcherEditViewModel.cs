using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Http;
using Newtonsoft.Json;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class ResearcherEditViewModel: PageViewModelBase
    {
        public IList<IFormFile> Files { get; set; }

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

        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Требуется выбрать год рождения")]
        public int BirthYear { get; set; }


        [Required(ErrorMessage = "Укажите E-mail")]
        [EmailAddress(ErrorMessage = "Поле E-mail содержит не допустимый адрес электронной почты.")]
        public string Email { get; set; }
        [EmailAddress(ErrorMessage = "Поле E-mail содержит не допустимый адрес электронной почты.")]
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
        public string Rewards { get; set; }
        public string Memberships { get; set; }
        public string Conferences { get; set; }

        public string ImageName { get; set; }
        public long? ImageSize { get; set; }
        public string ImageExtension { get; set; }
        public string ImageUrl { get; set; }

        public List<EducationEditViewModel> Educations { get; set; }
        public List<PublicationEditViewModel> Publications { get; set; }
        public List<string> Interests { get; set; } 
    }
}