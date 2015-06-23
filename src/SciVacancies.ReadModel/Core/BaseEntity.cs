using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Nest;

namespace SciVacancies.ReadModel.Core
{
    public class BaseEntity
    {
        [ElasticProperty(Name = "Id")]
        public Guid Guid { get; set; }
    }
}
