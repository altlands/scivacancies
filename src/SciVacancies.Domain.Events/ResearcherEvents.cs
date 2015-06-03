using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Events
{
    public class ResearcherEventBase : EventBase
    {
        public ResearcherEventBase() : base() { }

        public Guid ResearcherGuid { get; set; }
    }
    public class ResearcherCreated : ResearcherEventBase
    {
        public ResearcherCreated() : base() { }


    }
    public class ResearcherRemoved : ResearcherEventBase
    {
        public ResearcherRemoved() : base() { }


    }
}
