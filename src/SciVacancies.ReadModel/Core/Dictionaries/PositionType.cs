using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("PositionType")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class PositionType : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }

    }
}
