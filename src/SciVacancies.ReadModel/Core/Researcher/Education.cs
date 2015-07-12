using System;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("educations")]
    [PrimaryKey("guid", AutoIncrement = false)]
    public class Education : BaseEntity
    {
        public string city { get; set; }

        public string university_shortname { get; set; }

        public string faculty_shortname { get; set; }

        public DateTime? graduation_date { get; set; }

        public string degree { get; set; }

        public Guid researcher_guid { get; set; }
    }
}
