using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.MssqlDB.Handlers
{
    public class ResearcherCreatedHandler : EventBaseHandler<ResearcherCreated>
    {
        public ResearcherCreatedHandler(IDatabase db) : base(db) { }
        public override void Handle(ResearcherCreated msg)
        {
            
        }
    }
    public class ResearcherRemovedHandler : EventBaseHandler<ResearcherRemoved>
    {
        public ResearcherRemovedHandler(IDatabase db) : base(db) { }
        public override void Handle(ResearcherRemoved msg)
        {

        }
    }
}
