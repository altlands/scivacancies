using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.MssqlDB.Core
{
    [TableName("Applicants")]
    [PrimaryKey("Id", AutoIncrement = false)]
    public class Applicant:BaseEntity
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }

        public DateTime BirthDay { get; set; }
        public string Nationality { get; set; }

        public DateTime? UpdateDate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
