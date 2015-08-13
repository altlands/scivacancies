using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace SciVacancies.WebApp.ViewModels
{
    public class AccountOrganizationRegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Требуется ввести пароль")]
        //[PasswordPropertyText]
        public string Password { get; set; }
        [Required(ErrorMessage = "Требуется повторить пароль")]
        //[PasswordPropertyText]
        [Compare("Password", ErrorMessage = "Пароли отличаются")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Идентификатор аккаунта, к которому привязан агрегат
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Полное наименование
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// Полное наименование (на английском языке)
        /// </summary>
        public string NameEng { get; set; }

        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Сокращенное наименование (на английском языке)
        /// </summary>
        public string ShortNameEng { get; set; }

        /// <summary>
        /// Населенный пункт
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Почтовый адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Веб-сайт
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public string INN { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public string OGRN { get; set; }

        /// <summary>
        /// Руководитель
        /// </summary>
        public string HeadFirstName { get; set; }
        public string HeadLastName { get; set; }
        public string HeadPatronymic { get; set; } //HeadMiddleName

        /// <summary>
        /// Логотип организации
        /// </summary>
        public string ImageName { get; set; }
        public long? ImageSize { get; set; }
        public string ImageExtension { get; set; }
        public string ImageUrl { get; set; }

        /// <summary>
        /// Организационно-правовая форма организации
        /// </summary>
        public string OrgForm { get; set; }
        public int OrgFormId { get; set; }
        public Guid OrgFormGuid { get; set; }

        /// <summary>
        /// Количество опубликованных вакансий на данный момент
        /// </summary>
        public int PublishedVacancies { get; set; }

        /// <summary>
        /// ФОИВ
        /// </summary>
        public string Foiv { get; set; }
        public int FoivId { get; set; }
        public Guid FoivGuid { get; set; }


        /// <summary>
        /// Основной вид деятельности
        /// </summary>
        public string Activity { get; set; }
        public int ActivityId { get; set; }
        public Guid ActivityGuid { get; set; }

        /// <summary>
        /// Отрасли науки
        /// </summary>
        public List<int> ResearchDirections { get; set; }

        /// <summary>
        /// Сюда пишутся access_token и прочее
        /// </summary>
        public List<Claim> Claims { get; set; }
    }
}
