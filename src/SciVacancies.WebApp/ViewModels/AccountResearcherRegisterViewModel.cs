using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SciVacancies.WebApp.ViewModels
{
    /// <summary>
    /// класс для регистрации пользователя
    /// </summary>
    public class AccountResearcherRegisterViewModel
    {
        [Required]
        [DefaultValue(false)]
        public bool Agreement { get; set; }

        [Required(ErrorMessage = "Требуется ввести пароль")]
        //[PasswordPropertyText]
        public string Password { get; set; }
        [Required(ErrorMessage = "Требуется повторить пароль")]
        //[PasswordPropertyText]
        [Compare("Password", ErrorMessage = "Пароли отличаются")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Требуется ввести код скартинки")]
        public string Captcha { get; set; }


        [Required(ErrorMessage = "Требуется ввести логин")]
        [RegularExpression(@"^([a-zA-Z][\w.]+|[0-9][0-9_.]*[a-zA-Z]+[\w.]*)$", ErrorMessage = "Логин без точек и пробелов")]
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
        [Required(ErrorMessage = "Требуется заполнить поле Телефон")]
        [Phone(ErrorMessage = "Поле Телефон содержит не допустимый номер телефона.")]
        public string Phone { get; set; }
        [Phone(ErrorMessage = "Поле Добавить телефон содержит не допустимый номер телефона.")]
        public string ExtraPhone { get; set; }


        ///// <summary>
        ///// Сюда пишутся access_token  и прочее, если OAuth авторизация
        ///// </summary>
        //public List<Claim> Claims { get; set; }
    }
}
