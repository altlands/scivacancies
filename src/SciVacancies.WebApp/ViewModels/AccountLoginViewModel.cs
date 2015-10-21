using System.ComponentModel.DataAnnotations;
using SciVacancies.WebApp.Engine;

namespace SciVacancies.WebApp.ViewModels
{
    public class AccountLoginViewModel
    {

        [Required(ErrorMessage = "Требуется заполнить поле Логин (e-mail)")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Требуется ввести пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public AuthorizeUserTypes User { get; set; }
        public AuthorizeResourceTypes Resource { get; set; }
        public int UnreadNotificationCount { get; set; }
    }
}
