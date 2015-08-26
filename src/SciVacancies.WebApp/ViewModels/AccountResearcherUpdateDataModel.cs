using System;
using System.ComponentModel;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using SciVacancies.Domain.Core;

namespace SciVacancies.WebApp.ViewModels
{
    /// <summary>
    /// описание Исследователя (используоется для обновления информации об исследователе)
    /// </summary>
    public class ProfileResearcherUpdateDataModel
    {
        [Required(ErrorMessage = "Требуется ввести логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Требуется заполнить поле Имя")]
        public string FirstName { get; set; }
        //TODO: переименовать в MiddleName/SecondName
        public string Patronymic { get; set; }
        //TODO: переименовать SecondName в LastName
        [Required(ErrorMessage = "Требуется заполнить поле Фамилия")]
        public string SecondName { get; set; }
        //TODO: переименовать PreviousSecondName в PreviousLastName
        public string PreviousSecondName { get; set; }

        public string FirstNameEng { get; set; }
        //TODO: переименовать в MiddleNameEng/SecondNameEng
        public string PatronymicEng { get; set; }
        //TODO: переименовать SecondNameEng в LastNameEng
        public string SecondNameEng { get; set; }
        //TODO: переименовать PreviousSecondNameEng в PreviousLastNameEng
        public string PreviousSecondNameEng { get; set; }

        [Required(ErrorMessage = "Требуется выбрать год рождения")]
        public int BirthYear { get; set; }
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Требуется указать адрес электронной почты")]
        [EmailAddress(ErrorMessage = "Поле E-mail содержит не допустимый адрес электронной почты.")]
        public string Email { get; set; }
        [EmailAddress(ErrorMessage = "Поле Добавить E-mail содержит не допустимый адрес электронной почты.")]
        public string ExtraEmail { get; set; }
        [Required(ErrorMessage = "Требуется заполнить поле Телефон")]
        [Phone(ErrorMessage = "Поле Телефон содержит не допустимый номер телефона.")]
        public string Phone { get; set; }
        [Phone(ErrorMessage = "Поле Добавить телефон содержит не допустимый номер телефона.")]
        public string ExtraPhone { get; set; }

        /// <summary>
        /// научные интересы
        /// </summary>
        public string Interests { get; set; }

        /// <summary>
        /// временное поле для Образования, пока не перейдем на List[Education]
        /// </summary>
        public string Education { get; set; }

        public List<Publication> Publications { get; set; }

        /// <summary>
        /// награды
        /// </summary>
        public string Rewards { get; set; }

        public string Memberships { get; set; }


        //public string Nationality { get; set; }
        //public string ResearchActivity { get; set; }
        //public string TeachingActivity { get; set; }
        //public string OtherActivity { get; set; }
        //public string ScienceDegree { get; set; }
        //public string ScienceRank { get; set; }
        //public string Memberships { get; set; }
        //public string Conferences { get; set; }
        //public string Educations { get; set; }
    }
}
