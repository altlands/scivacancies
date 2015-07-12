using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("positiontypes")]
    [PrimaryKey("id", AutoIncrement = false)]
    public class PositionType
    {
        public int id { get; set; }

        public string title { get; set; }
    }
}
