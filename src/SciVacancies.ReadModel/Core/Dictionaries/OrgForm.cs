using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("OrgForm")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class OrgForm : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }

    }
}
