using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Events
{
    public class OrganizationEventBase : EventBase
    {
        public OrganizationEventBase() : base() { }

        public Guid OrganizationGuid { get; set; }
        public OrganizationDataModel Data { get; set; }
    }
    
    public class OrganizationCreated : OrganizationEventBase
    {
        public OrganizationCreated() : base() { }

    }
    
    public class OrganizationRemoved : OrganizationEventBase
    {
        public OrganizationRemoved() : base() { }
    }
    public class OrganizationUpdated : OrganizationEventBase
    {
        public OrganizationUpdated() : base() { }

     
    }
}
