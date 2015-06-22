using System;

namespace SciVacancies.WebApp.ViewModels
{
    public class AccountRegisterViewModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
