
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
        public string Password { get; set; }

        [Required(ErrorMessage = "Требуется повторить пароль")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли отличаются")]
        public string PasswordConfirm { get; set; }
    }
}
