using SciVacancies.ReadModel.Core;

using System.Collections.Generic;

using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SelectAllCriteriasQuery : IRequest<IEnumerable<Criteria>>
    {

    }
    public class SelectCriteriasForAutocompleteQuery : IRequest<IEnumerable<Criteria>>
    {
        public string Query { get; set; }
        public int Take { get; set; }
    }
    public class SelectCriteriasByParentIdQuery : IRequest<IEnumerable<Criteria>>
    {
        public int ParentId { get; set; }
    }

    public class SelectAllFoivsQuery : IRequest<IEnumerable<Foiv>>
    {

    }
    public class SelectFoivsForAutocompleteQuery : IRequest<IEnumerable<Foiv>>
    {
        public string Query { get; set; }
        public int Take { get; set; }
    }
    public class SelectFoivsByParentIdQuery : IRequest<IEnumerable<Foiv>>
    {
        public int ParentId { get; set; }
    }

    public class SelectAllOrgFormsQuery : IRequest<IEnumerable<OrgForm>>
    {

    }
    public class SelectOrgFormsForAutocompleteQuery : IRequest<IEnumerable<OrgForm>>
    {
        public string Query { get; set; }
        public int Take { get; set; }
    }

    public class SelectAllPositionTypesQuery : IRequest<IEnumerable<PositionType>>
    {

    }
    public class SelectPositionTypesForAutocompleteQuery : IRequest<IEnumerable<PositionType>>
    {
        public string Query { get; set; }
        public int Take { get; set; }
    }

    public class SelectAllRegionsQuery : IRequest<IEnumerable<Region>>
    {

    }
    public class SelectRegionsForAutocompleteQuery : IRequest<IEnumerable<Region>>
    {
        public string Query { get; set; }
        public int Take { get; set; }
    }

    public class SelectAllResearchDirectionsQuery : IRequest<IEnumerable<ResearchDirection>>
    {

    }
    public class SelectResearchDirectionsForAutocompleteQuery : IRequest<IEnumerable<ResearchDirection>>
    {
        public string Query { get; set; }
        public int Take { get; set; }
    }
    public class SelectResearchDirectionsByParentIdQuery : IRequest<IEnumerable<ResearchDirection>>
    {
        public int ParentId { get; set; }
    }
    public class SelectAllAttachmentTypesQuery : IRequest<IEnumerable<AttachmentType>>
    {

    }
}
