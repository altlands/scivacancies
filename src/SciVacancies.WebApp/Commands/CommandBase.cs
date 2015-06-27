using System;

namespace SciVacancies.WebApp.Commands
{
    public abstract class CommandBase
    {
        /// <summary>
        /// Id of command
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();
        /// <summary>
        /// TimeStamp of command creation
        /// </summary>
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
    }
}