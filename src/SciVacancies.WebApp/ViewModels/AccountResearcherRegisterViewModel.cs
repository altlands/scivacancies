using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Требуется заполнить поле Электронная почта")]
        [EmailAddress(ErrorMessage = "Поле Электронная почта не содержит допустимый адрес электронной почты.")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Поле Телефон не содержит допустимый номер телефона.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Требуется ввести пароль")]
        //[PasswordPropertyText]
        public string Password { get; set; }
        [Required(ErrorMessage = "Требуется повторить пароль")]
        //[PasswordPropertyText]
        [Compare("Password", ErrorMessage = "Пароли отличаются")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Требуется ввести код скартинки")]
        public string Captcha { get; set; }

    }
}
