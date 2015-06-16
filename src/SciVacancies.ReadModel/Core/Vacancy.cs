using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Vacancies")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Vacancy : BaseEntity
    {

    }
}
