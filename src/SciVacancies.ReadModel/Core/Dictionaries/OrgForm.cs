using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("d_orgforms")]
    [PrimaryKey("id", AutoIncrement = false)]
    public class OrgForm
    {
        public int id { get; set; }

        public string title { get; set; }
    }
}
