using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.WebApp.ViewModels
{
    public class AccountLoginViewModel
    {
        [Required]
        public bool IsResearcher { get; set; }

        [Required(ErrorMessage = "Требуется заполнить поле Логин (e-mail)")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
