using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class Request
    {
        public Guid RequestGuid { get; set; }
        public Guid CompetitionGuid { get; set; }
        public RequestStatus Status { get; set; }
    }
}
