using System;

using MediatR;

namespace SciVacancies.Domain.Commands
{
    public abstract class CommandBase
    {
        private Guid id { get; set; }
        private DateTime timeStamp { get; set; }

        public CommandBase()
        {
            id = Guid.NewGuid();
            timeStamp = DateTime.UtcNow;
        }

        public Guid Id { get { return id; } }
        public DateTime TimeStamp { get { return timeStamp; } }
    }
}