using System;
using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Publications")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Publication : BaseEntity
    {
        public Guid ResearcherGuid { get; set; }

        public string Name { get; set; }
    }
}
