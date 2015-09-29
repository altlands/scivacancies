using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Queries
{
    public class SingleOrganizationQuery : IRequest<Organization>
    {
        public Guid OrganizationGuid { get; set; }
    }

    public class CountOrganizationsQuery : IRequest<int>
    {
    }
    public class SelectOrganizationsForAutocompleteQuery : IRequest<IEnumerable<Organization>>
    {
        public string Query { get; set; }
        public int Take { get; set; }
    }
    public class SelectPagedOrganizationsQuery : IRequest<Page<Organization>>
    {
        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }
        public string NameFilterValue { get; set; }
        public string AddressFilterValue { get; set; }
    }
    public class SelectOrganizationsByGuidsQuery : IRequest<IEnumerable<Organization>>
    {
        public IEnumerable<Guid> OrganizationGuids { get; set; }
    }

    public class SelectOrganizationResearchDirectionsQuery:IRequest<IEnumerable<ResearchDirection>>
    {
        public Guid OrganizationGuid { get; set; }
    }
}
