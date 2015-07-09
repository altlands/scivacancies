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

        public int PositionTypeId { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string ExtraEmail { get; set; }
        public string Phone { get; set; }
        public string ExtraPhone { get; set; }

        ///// <summary>
        ///// временное поле для Образования, пока не перейдем на List[Education]
        ///// </summary>
        //public string Education { get; set; }

        public string ResearchActivity { get; set; }
        public string TeachingActivity { get; set; }
        public string OtherActivity { get; set; }

        public string ScienceDegree { get; set; }
        public string AcademicStatus { get; set; }
        public string Rewards { get; set; }
        public string Memberships { get; set; }
        public string Conferences { get; set; }

        public VacancyApplicationStatus Status { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? SentDate { get; set; }

        //public string CoveringLetter { get; set; }
    }
}
