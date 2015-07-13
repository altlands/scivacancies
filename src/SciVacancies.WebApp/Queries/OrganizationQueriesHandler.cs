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
        IRequestHandler<SelectPagedOrganizationsQuery, Page<Organization>>
    {
        private readonly IDatabase _db;

        public OrganizationQueriesHandler(IDatabase db)
        {
            _db = db;
        }

        public Organization Handle(SingleOrganizationQuery message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");

            Organization organization = _db.SingleOrDefaultById<Organization>(message.OrganizationGuid);

            return organization;
        }
        public IEnumerable<Organization> Handle(SelectOrganizationsForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Organization> organizations;
            if (message.Take != 0)
            {
                organizations = _db.FetchBy<Organization>(f => f.Where(w => w.name.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                organizations = _db.FetchBy<Organization>(f => f.Where(w => w.name.Contains(message.Query)));
            }

            return organizations;
        }
        public Page<Organization> Handle(SelectPagedOrganizationsQuery message)
        {
            Page<Organization> organizations = _db.Page<Organization>(message.PageIndex, message.PageSize, new Sql("SELECT o.* FROM org_organizations o ORDER BY o.guid DESC"));

            return organizations;
        }
    }
}