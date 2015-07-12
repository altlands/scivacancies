using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("d_positiontypes")]
    [PrimaryKey("id", AutoIncrement = false)]
    public class PositionType
    {
        public int id { get; set; }

        public string title { get; set; }
    }
}
