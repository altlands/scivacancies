using System;

namespace SciVacancies.Domain.Aggregates.Commands
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
        /// <summary>
        /// Id of command
        /// </summary>
        public Guid Id { get { return id; } }
        /// <summary>
        /// Time of command creation
        /// </summary>
        public DateTime TimeStamp { get { return timeStamp; } }
    }
}