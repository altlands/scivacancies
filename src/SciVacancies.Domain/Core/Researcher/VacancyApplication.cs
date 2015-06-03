using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class VacancyApplication
    {
        public Guid VacancyApplicationGuid { get; set; }
        public Guid VacancyGuid { get; set; }
        public VacancyApplicationStatus Status { get; set; }
    }
}
