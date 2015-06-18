using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Attachments")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Attachment:BaseEntity
    {
        public DateTime CreationdDate { get; set; }
    }
}
