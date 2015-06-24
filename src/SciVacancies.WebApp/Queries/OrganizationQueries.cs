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
    public class SelectOrganizationsByTitleQuery : IRequest<List<Organization>>
    {
        public string Title { get; set; }
        public int Count { get; set; }
    }
    public class SelectPagedOrganizationsQuery : IRequest<Page<Organization>>
    {
        public string OrderBy { get; set; }
        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string NameFilterValue { get; set; }
        public string AddressFilterValue { get; set; }
    }
}
