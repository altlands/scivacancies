using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Notifications")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Notification:BaseEntity
    {

    }
}
