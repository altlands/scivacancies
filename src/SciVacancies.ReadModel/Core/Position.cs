using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Positions")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Position : BaseEntity
    {
    }
}
