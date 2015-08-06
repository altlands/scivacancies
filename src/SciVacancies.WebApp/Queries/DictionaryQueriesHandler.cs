using SciVacancies.ReadModel.Core;
using SciVacancies.ReadModel;

using System;
using System.Collections.Generic;
using System.Linq;

using MediatR;
using NPoco;
using Nest;

namespace SciVacancies.WebApp.Queries
{
    public class DictionaryQueriesHandler :
        IRequestHandler<SelectAllCriteriasQuery, IEnumerable<Criteria>>,
        IRequestHandler<SelectCriteriasForAutocompleteQuery, IEnumerable<Criteria>>,
        IRequestHandler<SelectCriteriasByParentIdQuery, IEnumerable<Criteria>>,

        IRequestHandler<SelectAllFoivsQuery, IEnumerable<Foiv>>,
        IRequestHandler<SelectFoivsForAutocompleteQuery, IEnumerable<Foiv>>,
        IRequestHandler<SelectFoivsByParentIdQuery, IEnumerable<Foiv>>,

        IRequestHandler<SelectAllOrgFormsQuery, IEnumerable<OrgForm>>,
        IRequestHandler<SelectOrgFormsForAutocompleteQuery, IEnumerable<OrgForm>>,

        IRequestHandler<SelectAllPositionTypesQuery, IEnumerable<PositionType>>,
        IRequestHandler<SelectPositionTypesForAutocompleteQuery, IEnumerable<PositionType>>,

        IRequestHandler<SelectAllRegionsQuery, IEnumerable<Region>>,
        IRequestHandler<SelectRegionsForAutocompleteQuery, IEnumerable<Region>>,

        IRequestHandler<SelectAllResearchDirectionsQuery, IEnumerable<ResearchDirection>>,
        IRequestHandler<SelectResearchDirectionsForAutocompleteQuery, IEnumerable<ResearchDirection>>,
        IRequestHandler<SelectResearchDirectionsByParentIdQuery, IEnumerable<ResearchDirection>>
    {
        private readonly IDatabase _db;

        public DictionaryQueriesHandler(IDatabase db)
        {
            _db = db;
        }

        public IEnumerable<Criteria> Handle(SelectAllCriteriasQuery message)
        {
            IEnumerable<Criteria> criterias = _db.Fetch<Criteria>();

            return criterias;
        }
        public IEnumerable<Criteria> Handle(SelectCriteriasForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Criteria> criterias;
            if (message.Take != 0)
            {
                criterias = _db.FetchBy<Criteria>(f => f.Where(w => w.title.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                criterias = _db.FetchBy<Criteria>(f => f.Where(w => w.title.Contains(message.Query)));
            }

            return criterias;
        }
        public IEnumerable<Criteria> Handle(SelectCriteriasByParentIdQuery message)
        {
            if (message.ParentId == 0) throw new ArgumentNullException($"ParentId is empty or 0: {message.ParentId}");

            IEnumerable<Criteria> criterias = _db.FetchBy<Criteria>(f => f.Where(w => w.parent_id == message.ParentId));

            return criterias;
        }

        public IEnumerable<Foiv> Handle(SelectAllFoivsQuery message)
        {
            IEnumerable<Foiv> foivs = _db.Fetch<Foiv>();

            return foivs;
        }
        public IEnumerable<Foiv> Handle(SelectFoivsForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Foiv> foivs;
            if (message.Take != 0)
            {
                foivs = _db.FetchBy<Foiv>(f => f.Where(w => w.title.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                foivs = _db.FetchBy<Foiv>(f => f.Where(w => w.title.Contains(message.Query)));
            }

            return foivs;
        }
        public IEnumerable<Foiv> Handle(SelectFoivsByParentIdQuery message)
        {
            if (message.ParentId == 0) throw new ArgumentNullException($"ParentId is empty or 0: {message.ParentId}");

            IEnumerable<Foiv> foivs = _db.FetchBy<Foiv>(f => f.Where(w => w.parent_id == message.ParentId));

            return foivs;
        }

        public IEnumerable<OrgForm> Handle(SelectAllOrgFormsQuery message)
        {
            IEnumerable<OrgForm> orgForms = _db.Fetch<OrgForm>();

            return orgForms;
        }
        public IEnumerable<OrgForm> Handle(SelectOrgFormsForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<OrgForm> orgForms;
            if (message.Take != 0)
            {
                orgForms = _db.FetchBy<OrgForm>(f => f.Where(w => w.title.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                orgForms = _db.FetchBy<OrgForm>(f => f.Where(w => w.title.Contains(message.Query)));
            }

            return orgForms;
        }

        public IEnumerable<PositionType> Handle(SelectAllPositionTypesQuery message)
        {
            IEnumerable<PositionType> positionTypes = _db.Fetch<PositionType>();

            return positionTypes;
        }
        public IEnumerable<PositionType> Handle(SelectPositionTypesForAutocompleteQuery message)
        {
            if (string.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<PositionType> positionTypes;
            if (message.Take != 0)
            {
                positionTypes = _db.FetchBy<PositionType>(f => f.Where(w => w.title.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                positionTypes = _db.FetchBy<PositionType>(f => f.Where(w => w.title.Contains(message.Query)));
            }

            return positionTypes;
        }

        public IEnumerable<Region> Handle(SelectAllRegionsQuery message)
        {
            IEnumerable<Region> regions = _db.Fetch<Region>();

            return regions;
        }
        public IEnumerable<Region> Handle(SelectRegionsForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Region> regions;
            if (message.Take != 0)
            {
                regions = _db.FetchBy<Region>(f => f.Where(w => w.title.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                regions = _db.FetchBy<Region>(f => f.Where(w => w.title.Contains(message.Query)));
            }

            return regions;
        }

        public IEnumerable<ResearchDirection> Handle(SelectAllResearchDirectionsQuery message)
        {
            IEnumerable<ResearchDirection> researchDirections = _db.Fetch<ResearchDirection>();

            return researchDirections;
        }
        public IEnumerable<ResearchDirection> Handle(SelectResearchDirectionsForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<ResearchDirection> researchDirections;
            if (message.Take != 0)
            {
                researchDirections = _db.FetchBy<ResearchDirection>(f => f.Where(w => w.title.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                researchDirections = _db.FetchBy<ResearchDirection>(f => f.Where(w => w.title.Contains(message.Query)));
            }

            return researchDirections;
        }
        public IEnumerable<ResearchDirection> Handle(SelectResearchDirectionsByParentIdQuery message)
        {
            if (message.ParentId == 0) throw new ArgumentNullException($"ParentId is empty or 0: {message.ParentId}");

            IEnumerable<ResearchDirection> researchDirections = _db.FetchBy<ResearchDirection>(f => f.Where(w => w.parent_id == message.ParentId));

            return researchDirections;
        }

    }
    
}
