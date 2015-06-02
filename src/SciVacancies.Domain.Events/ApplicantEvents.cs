using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Events
{
    public class ApplicantEventBase : EventBase
    {
        public ApplicantEventBase() : base() { }

        public Guid ApplicantGuid { get; set; }
    }
    public class ApplicantCreated : ApplicantEventBase
    {
        public ApplicantCreated() : base() { }


    }
    public class ApplicantRemoved : ApplicantEventBase
    {
        public ApplicantRemoved() : base() { }


    }
}
