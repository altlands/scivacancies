using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.MssqlDB.Core
{
    [TableName("Requests")]
    [PrimaryKey("Id", AutoIncrement = false)]
    public class Request:BaseEntity
    {
        public Guid CompetitionGuid { get; set; }
        public Guid ApplicantGuid { get; set; }
        public string CoveringLetter { get; set; }
    }
}
