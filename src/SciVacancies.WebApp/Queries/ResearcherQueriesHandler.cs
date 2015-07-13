using SciVacancies.ReadModel.Core;

using System;

using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class ResearcherQueriesHandler :
        IRequestHandler<SingleResearcherQuery, Researcher>
    {
        private readonly IDatabase _db;

        public ResearcherQueriesHandler(IDatabase db)
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