using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class ResearcherDataModel
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }

        public string FirstNameEng { get; set; }
        public string SecondNameEng { get; set; }
        public string PatronymicEng { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }
        public string ExtraEmail { get; set; }

        public string Phone { get; set; }
        public string ExtraPhone { get; set; }

        public string Nationality { get; set; }

        public DateTime RegistrationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime LastEntryDate { get; set; }
    }
}
