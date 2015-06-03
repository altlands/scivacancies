using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class Publication
    {
        public Guid PublicationGuid { get; set; }
        public string Name { get; set; }
    }
}
