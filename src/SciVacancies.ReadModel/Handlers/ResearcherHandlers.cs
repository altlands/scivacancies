using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Handlers
{
    public class ResearcherCreatedHandler : EventBaseHandler<ResearcherCreated>
    {
        public ResearcherCreatedHandler(IDatabase db) : base(db) { }
        public override void Handle(ResearcherCreated msg)
        {
            Researcher researcher = new Researcher()
            {
                Guid = msg.ResearcherGuid,
                CreationDate = msg.TimeStamp
            };

            _db.Insert(researcher);
        }
    }
    public class ResearcherRemovedHandler : EventBaseHandler<ResearcherRemoved>
    {
        public ResearcherRemovedHandler(IDatabase db) : base(db) { }
        public override void Handle(ResearcherRemoved msg)
        {
            _db.Delete<Researcher>(msg.ResearcherGuid);
        }
    }
}
