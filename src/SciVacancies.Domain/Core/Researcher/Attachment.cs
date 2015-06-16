using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class Attachment
    {
        public Guid AttachmentGuid { get; set; }

        public string Name { get; set; }
        public string Size { get; set; }
        public string Url { get; set; }
    }
}
