using System;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("res_publications")]
    [PrimaryKey("guid", AutoIncrement = false)]
    public class Publication : BaseEntity
    {
        public string name { get; set; }

        public Guid researcher_guid { get; set; }

        public string type { get; set; }

        public string ext_id { get; set; }

        public string authors { get; set; }

        public DateTime? updated { get; set; }

    }
}
