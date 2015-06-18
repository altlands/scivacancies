using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Criteria")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Criteria : BaseEntity
    {
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }

    }
}
