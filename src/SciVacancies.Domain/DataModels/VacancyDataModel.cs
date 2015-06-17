using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.DataModels
{
    public class VacancyDataModel
    {
        public Guid WinnerGuid { get; set; }
        public Guid PretenderGuid { get; set; }
    }
}
