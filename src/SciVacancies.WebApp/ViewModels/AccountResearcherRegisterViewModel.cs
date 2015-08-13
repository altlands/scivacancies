using System;
using System.ComponentModel;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SciVacancies.WebApp.ViewModels
{
    public class AccountResearcherRegisterViewModel
    {
        [Required]
        [DefaultValue(false)]
        public bool Agreement { get; set; }
        [Required(ErrorMessage = "Требуется ввести логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Требуется заоплнить поле Имя")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Требуется заоплнить поле Фамилия")]
        public string SecondName { get; set; }
        public string Patronymic { get; set; }

        public string FirstNameEng { get; set; }
        public string SecondNameEng { get; set; }
        public string PatronymicEng { get; set; }

        [Required(ErrorMessage = "Требуется выбрать год рождения")]
        public int BirthYear { get; set; }
        //public DateTime BirthDate { get; set; }

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

        [Required(ErrorMessage = "Требуется ввести пароль")]
        //[PasswordPropertyText]
        public string Password { get; set; }
        [Required(ErrorMessage = "Требуется повторить пароль")]
        //[PasswordPropertyText]
        [Compare("Password", ErrorMessage = "Пароли отличаются")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Требуется ввести код скартинки")]
        public string Captcha { get; set; }

        /// <summary>
        /// временное поле для Образования, пока не перейдем на List[Education]
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// Сюда пишутся access_token  и прочее, если OAuth авторизация
        /// </summary>
        public List<Claim> Claims { get; set; }
    }
}
