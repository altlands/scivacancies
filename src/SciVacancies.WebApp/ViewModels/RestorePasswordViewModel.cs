
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SciVacancies.WebApp.ViewModels
{
    public class RestorePasswordViewModel
    {
        public string ResetToken { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Требуется ввести пароль")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Длинна пароля должна быть не менее 6 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Требуется повторить пароль")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли отличаются")]
        [MinLength(6, ErrorMessage = "Длинна пароля должна быть не менее 6 символов")]
        public string PasswordConfirm { get; set; }
    }
}
