using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;

using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class OrganizationQueriesHandler :
        IRequestHandler<SingleOrganizationQuery, Organization>,
        IRequestHandler<SelectOrganizationsForAutocompleteQuery, IEnumerable<Organization>>,
        IRequestHandler<SelectPagedOrganizationsQuery, Page<Organization>>,
        IRequestHandler<SelectOrganizationsByGuidsQuery, IEnumerable<Organization>>,
        IRequestHandler<SelectOrganizationResearchDirectionsQuery, IEnumerable<ResearchDirection>>
    {
        private readonly IDatabase _db;

        public OrganizationQueriesHandler(IDatabase db)
        {
            _db = db;
        }

        public Organization Handle(SingleOrganizationQuery msg)
        {
            if (msg.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {msg.OrganizationGuid}");

            Organization organization = _db.SingleOrDefaultById<Organization>(msg.OrganizationGuid);

            return organization;
        }
        public IEnumerable<Organization> Handle(SelectOrganizationsForAutocompleteQuery msg)
        {
            if (String.IsNullOrEmpty(msg.Query)) throw new ArgumentNullException($"Query is empty: {msg.Query}");

            IEnumerable<Organization> organizations;
            if (msg.Take != 0)
            {
                organizations = _db.FetchBy<Organization>(f => f.Where(w => w.name.Contains(msg.Query) && w.status != OrganizationStatus.Removed)).Take(msg.Take);
            }
            else
            {
                organizations = _db.FetchBy<Organization>(f => f.Where(w => w.name.Contains(msg.Query) && w.status != OrganizationStatus.Removed));
            }

            return organizations;
        }
        public Page<Organization> Handle(SelectPagedOrganizationsQuery msg)
        {
            Page<Organization> organizations = _db.Page<Organization>(msg.PageIndex, msg.PageSize, new Sql("SELECT o.* FROM org_organizations o WHERE o.status != @0 ORDER BY o.guid DESC", OrganizationStatus.Removed));

            return organizations;
        }
        public IEnumerable<Organization> Handle(SelectOrganizationsByGuidsQuery msg)
        {
            IEnumerable<Organization> organizations = _db.Fetch<Organization>(new Sql($"SELECT o.* FROM org_organizations o WHERE o.guid IN (@0) AND o.status != @1 ORDER BY o.guid DESC", msg.OrganizationGuids, OrganizationStatus.Removed));

            return organizations;
        }

        public IEnumerable<ResearchDirection> Handle(SelectOrganizationResearchDirectionsQuery msg)
        {
            IEnumerable<ResearchDirection> orgResearchDirections = _db.Fetch<ResearchDirection>(new Sql($"SELECT * FROM org_researchdirections ors, d_researchdirections drs WHERE ors.organization_guid = @0 AND ors.researchdirection_id = drs.id", msg.OrganizationGuid));

            return orgResearchDirections;
        }
    }
}