using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("VacancyApplications")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class VacancyApplication : BaseEntity
    {
        public Guid VacancyGuid { get; set; }
        public Guid ResearcherGuid { get; set; }

        public VacancyApplicationStatus Status { get; set; }

        public DateTime CreationdDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        //public string CoveringLetter { get; set; }
    }
}
