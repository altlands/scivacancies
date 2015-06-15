using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Events
{
    public class PositionEventBase : EventBase
    {
        public PositionEventBase() : base() { }

        public Guid PositionGuid { get; set; }
        public Guid OrganizationGuid { get; set; }
    }

    public class PositionCreated : PositionEventBase
    {
        public PositionCreated() : base() { }

        public PositionDataModel Data { get; set; }
    }

    public class PositionUpdated : PositionEventBase
    {
        public PositionUpdated() : base() { }

        public PositionDataModel Data { get; set; }
    }

    public class PositionRemoved : PositionEventBase
    {
        public PositionRemoved() : base() { }
    }
}
