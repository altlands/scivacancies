using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Regions")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Region : BaseEntity
    {
        public int Id { get; set; }
        public int FedDistrictId { get; set; }
        public string Title { get; set; }
        public int OsmId { get; set; }
        public string Okato { get; set; }
        public string Slug { get; set; }
        public int Code { get; set; }

    }
}
