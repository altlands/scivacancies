using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

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

        public IEnumerable<Publication> Handle(SelectResearcherPublicationsQuery msg)
        {
            IEnumerable<Publication> resPublications = _db.Fetch<Publication>(new Sql($"SELECT * FROM res_publications rp WHERE rp.researcher_guid = @0", msg.ResearcherGuid));

            return resPublications;
        }
        public IEnumerable<Education> Handle(SelectResearcherEducationsQuery msg)
        {
            IEnumerable<Education> resEducations = _db.Fetch<Education>(new Sql($"SELECT * FROM res_educations re WHERE re.researcher_guid = @0", msg.ResearcherGuid));

            return resEducations;
        }
    }
}