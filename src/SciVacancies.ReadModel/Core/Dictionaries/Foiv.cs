using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("foivs")]
    [PrimaryKey("id", AutoIncrement = false)]
    public class Foiv
    {
        public int id { get; set; }

        public int? parent_id { get; set; }

        public string title { get; set; }

        public string shorttitle { get; set; }
    }
}
