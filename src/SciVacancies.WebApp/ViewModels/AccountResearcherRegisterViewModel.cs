using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;

namespace SciVacancies.WebApp.ViewModels
{
    /// <summary>
    /// класс для регистрации пользователя
    /// </summary>
    public class AccountResearcherRegisterViewModel : ProfileResearcherUpdateDataModel
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

        /// <summary>
        /// Сюда пишутся access_token  и прочее, если OAuth авторизация
        /// </summary>
        public List<Claim> Claims { get; set; }
    }
}
