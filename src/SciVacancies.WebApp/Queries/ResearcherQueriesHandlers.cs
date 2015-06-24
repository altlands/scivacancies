using SciVacancies.ReadModel.Core;

using System;

using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SingleResearcherQueryHandler : IRequestHandler<SingleResearcherQuery, Researcher>
    {
        private readonly IDatabase _db;

        public SingleResearcherQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Researcher Handle(SingleResearcherQuery message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");

            Researcher researcher = _db.SingleOrDefaultById<Researcher>(message.ResearcherGuid);

            return researcher;
        }
    }
}