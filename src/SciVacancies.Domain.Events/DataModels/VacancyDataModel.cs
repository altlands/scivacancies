using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Events
{
    public class VacancyDataModel
    {
        public Guid WinnerGuid { get; set; }
        public Guid PretenderGuid { get; set; }
    }
}
