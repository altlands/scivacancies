using System;

namespace SciVacancies.Domain.Events
{    
    public abstract class EventBase
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
