using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Nest;
using NPoco;

namespace SciVacancies.ReadModel.Core
{
    public class BaseEntity
    {
        [ElasticProperty(OptOut = true)]
        public Guid Guid { get; set; }
    }
}
