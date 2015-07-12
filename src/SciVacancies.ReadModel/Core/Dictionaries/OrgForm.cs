using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("orgforms")]
    [PrimaryKey("id", AutoIncrement = false)]
    public class OrgForm
    {
        public int id { get; set; }

        public string title { get; set; }
    }
}
