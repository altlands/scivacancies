using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Organizations")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Organization : BaseEntity
    {

        /// <summary>
        /// Полное наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Полное наименование (на английском языке)
        /// </summary>
        public string NameEng { get; set; }

        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Сокращенное наименование (на английском языке)
        /// </summary>
        public string ShortNameEng { get; set; }

        public DateTime CreationdDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
