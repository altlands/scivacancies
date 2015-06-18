using SciVacancies.Domain.Enums;
using SciVacancies.Domain.DataModels;

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

        public VacancyApplicationDataModel Data { get; set; }

        //public List<Publication> Publications { get; set; }
        //public List<AttachedFile> AttachedFiles { get; set; }
        //public string CoveringLetter { get; set; }

        public VacancyApplicationStatus Status { get; set; }
    }
}
