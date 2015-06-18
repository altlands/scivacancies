using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Foiv")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Foiv : BaseEntity
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }

    }
}
