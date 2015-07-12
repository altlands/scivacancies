using System;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("publications")]
    [PrimaryKey("guid", AutoIncrement = false)]
    public class Publication : BaseEntity
    {
        public string title { get; set; }

        public Guid researcher_guid { get; set; }
    }
}
