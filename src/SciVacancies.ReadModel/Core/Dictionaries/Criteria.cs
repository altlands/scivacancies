using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("criterias")]
    [PrimaryKey("id", AutoIncrement = false)]
    public class Criteria
    {
        public int id { get; set; }

        public int? parent_id { get; set; }

        public string title { get; set; }

        public string code { get; set; }
    }
}
