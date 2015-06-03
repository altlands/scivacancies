using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Researchers")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Researcher:BaseEntity
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }

        public DateTime BirthDay { get; set; }
        public string Nationality { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
