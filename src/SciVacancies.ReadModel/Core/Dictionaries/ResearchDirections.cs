using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("ResearchDirections")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class ResearchDirections : BaseEntity
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string TitleEng { get; set; }
        public int Lft { get; set; }
        public int Rgt { get; set; }
        public int Lvl { get; set; }
        public string OecdCode { get; set; }
        public string WosCode { get; set; }

    }
}
