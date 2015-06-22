using System;

namespace SciVacancies.Domain.Aggregates.Commands
{
    public class CommandBase 
    {
        public CommandBase()
        {
            Id = Guid.NewGuid();
        }
        /// <summary>
        /// Id of command. Usefull to track command flow through application components
        /// </summary>
        public Guid Id { get; set; }
    }    
}
