using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("d_researchdirections")]
    [PrimaryKey("id", AutoIncrement = false)]
    public class ResearchDirection
    {
        public int id { get; set; }

        public int? parent_id { get; set; }

        public string title { get; set; }

        public string title_eng { get; set; }

        public int? lft { get; set; }

        public int? rgt { get; set; }

        public int? lvl { get; set; }

        public string oecd_code { get; set; }

        public string wos_code { get; set; }

        public int? root_id { get; set; }
    }
}
