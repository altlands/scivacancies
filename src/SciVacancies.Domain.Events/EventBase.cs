using System;
using MediatR;

namespace SciVacancies.Domain.Events
{
    public abstract class EventBase : INotification
    {        
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
    }
}
