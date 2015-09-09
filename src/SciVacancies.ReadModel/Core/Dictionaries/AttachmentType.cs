using NPoco;

namespace SciVacancies.ReadModel.Core.Dictionaries
{
    [TableName("d_attachmenttypes")]
    [PrimaryKey("id", AutoIncrement = true)]
    public class AttachmentType
    {
        public int id { get; set; }
        public string title { get; set; }
    }
}