using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("d_regions")]
    [PrimaryKey("id", AutoIncrement = false)]
    public class Region
    {
        public int id { get; set; }

        public int? feddistrict_id { get; set; }

        public string title { get; set; }

        public int? osm_id { get; set; }

        public string okato { get; set; }

        public string slug { get; set; }

        public int? code { get; set; }
    }
}
