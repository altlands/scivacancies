using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class AttachedFile
    {
        public Guid AttachedFileGuid { get; set; }

        public string Name { get; set; }
        public string Size { get; set; }
        public string Url { get; set; }
    }
}
