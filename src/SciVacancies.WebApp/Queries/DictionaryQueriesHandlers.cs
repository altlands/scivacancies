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
    //TODO - подтягивание словарей из эластика
    public class SelectAllPositionTypesQueryHandler : IRequestHandler<SelectAllPositionTypesQuery, IEnumerable<PositionType>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectAllPositionTypesQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<PositionType> Handle(SelectAllPositionTypesQuery message)
        {
            IEnumerable<PositionType> positionTypes = _db.Fetch<PositionType>();

            return positionTypes;
        }
    }
    public class SelectPositionTypesForAutocompleteQueryHandler : IRequestHandler<SelectPositionTypesForAutocompleteQuery, IEnumerable<PositionType>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectPositionTypesForAutocompleteQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<PositionType> Handle(SelectPositionTypesForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<PositionType> positionTypes;
            if (message.Take != 0)
            {
                positionTypes = _db.FetchBy<PositionType>(f => f.Where(w => w.Title.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                positionTypes = _db.FetchBy<PositionType>(f => f.Where(w => w.Title.Contains(message.Query)));
            }

            return positionTypes;
        }
    }
    public class SelectAllActivitiesQueryHandler : IRequestHandler<SelectAllActivitiesQuery, IEnumerable<Activity>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectAllActivitiesQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<Activity> Handle(SelectAllActivitiesQuery message)
        {
            IEnumerable<Activity> activities = _db.Fetch<Activity>();

            return activities;
        }
    }
    public class SelectActivitiesForAutocompleteQueryHandler : IRequestHandler<SelectActivitiesForAutocompleteQuery, IEnumerable<Activity>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectActivitiesForAutocompleteQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<Activity> Handle(SelectActivitiesForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Activity> activities;
            if (message.Take != 0)
            {
                activities = _db.FetchBy<Activity>(f => f.Where(w => w.Title.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                activities = _db.FetchBy<Activity>(f => f.Where(w => w.Title.Contains(message.Query)));
            }

            return activities;
        }
    }
    public class SelectAllFoivsQueryHandler : IRequestHandler<SelectAllFoivsQuery, IEnumerable<Foiv>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectAllFoivsQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<Foiv> Handle(SelectAllFoivsQuery message)
        {
            IEnumerable<Foiv> foivs = _db.Fetch<Foiv>();

            return foivs;
        }
    }
    public class SelectFoivsForAutocompleteQueryHandler : IRequestHandler<SelectFoivsForAutocompleteQuery, IEnumerable<Foiv>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectFoivsForAutocompleteQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<Foiv> Handle(SelectFoivsForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Foiv> foivs;
            if (message.Take != 0)
            {
                foivs = _db.FetchBy<Foiv>(f => f.Where(w => w.Title.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                foivs = _db.FetchBy<Foiv>(f => f.Where(w => w.Title.Contains(message.Query)));
            }

            return foivs;
        }
    }
    public class SelectFoivsByParentIdQueryHandler : IRequestHandler<SelectFoivsByParentIdQuery, IEnumerable<Foiv>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectFoivsByParentIdQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<Foiv> Handle(SelectFoivsByParentIdQuery message)
        {
            if (message.ParentId == null || message.ParentId == 0) throw new ArgumentNullException($"ParentId is empty: {message.ParentId}");

            IEnumerable<Foiv> foivs = _db.FetchBy<Foiv>(f => f.Where(w => w.ParentId == message.ParentId));

            return foivs;
        }
    }
    public class SelectAllCriteriasQueryHandler : IRequestHandler<SelectAllCriteriasQuery, IEnumerable<Criteria>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectAllCriteriasQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<Criteria> Handle(SelectAllCriteriasQuery message)
        {
            IEnumerable<Criteria> criterias = _db.Fetch<Criteria>();

            return criterias;
        }
    }
    public class SelectCriteriasForAutocompleteQueryHandler : IRequestHandler<SelectCriteriasForAutocompleteQuery, IEnumerable<Criteria>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectCriteriasForAutocompleteQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<Criteria> Handle(SelectCriteriasForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Criteria> criterias;
            if (message.Take != 0)
            {
                criterias = _db.FetchBy<Criteria>(f => f.Where(w => w.Title.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                criterias = _db.FetchBy<Criteria>(f => f.Where(w => w.Title.Contains(message.Query)));
            }

            return criterias;
        }
    }
    public class SelectCriteriasByParentIdQueryHandler : IRequestHandler<SelectCriteriasByParentIdQuery, IEnumerable<Criteria>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectCriteriasByParentIdQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<Criteria> Handle(SelectCriteriasByParentIdQuery message)
        {
            if (message.ParentId == null || message.ParentId == 0) throw new ArgumentNullException($"ParentId is empty: {message.ParentId}");

            IEnumerable<Criteria> criterias = _db.FetchBy<Criteria>(f => f.Where(w => w.ParentId == message.ParentId));

            return criterias;
        }
    }
    public class SelectAllOrgFormsQueryHandler : IRequestHandler<SelectAllOrgFormsQuery, IEnumerable<OrgForm>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectAllOrgFormsQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<OrgForm> Handle(SelectAllOrgFormsQuery message)
        {
            IEnumerable<OrgForm> orgForms = _db.Fetch<OrgForm>();

            return orgForms;
        }
    }
    public class SelectOrgFormsForAutocompleteQueryHandler : IRequestHandler<SelectOrgFormsForAutocompleteQuery, IEnumerable<OrgForm>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectOrgFormsForAutocompleteQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<OrgForm> Handle(SelectOrgFormsForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<OrgForm> orgForms;
            if (message.Take != 0)
            {
                orgForms = _db.FetchBy<OrgForm>(f => f.Where(w => w.Title.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                orgForms = _db.FetchBy<OrgForm>(f => f.Where(w => w.Title.Contains(message.Query)));
            }

            return orgForms;
        }
    }
    public class SelectAllRegionsQueryHandler : IRequestHandler<SelectAllRegionsQuery, IEnumerable<Region>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectAllRegionsQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<Region> Handle(SelectAllRegionsQuery message)
        {
            IEnumerable<Region> regions = _db.Fetch<Region>();

            return regions;
        }
    }
    public class SelectRegionsForAutocompleteQueryHandler : IRequestHandler<SelectRegionsForAutocompleteQuery, IEnumerable<Region>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectRegionsForAutocompleteQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<Region> Handle(SelectRegionsForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Region> regions;
            if (message.Take != 0)
            {
                regions = _db.FetchBy<Region>(f => f.Where(w => w.Title.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                regions = _db.FetchBy<Region>(f => f.Where(w => w.Title.Contains(message.Query)));
            }

            return regions;
        }
    }
    public class SelectAllResearchDirectionsQueryHandler : IRequestHandler<SelectAllResearchDirectionsQuery, IEnumerable<ResearchDirection>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectAllResearchDirectionsQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<ResearchDirection> Handle(SelectAllResearchDirectionsQuery message)
        {
            IEnumerable<ResearchDirection> researchDirections = _db.Fetch<ResearchDirection>();

            return researchDirections;
        }
    }
    public class SelectResearchDirectionsForAutocompleteQueryHandler : IRequestHandler<SelectResearchDirectionsForAutocompleteQuery, IEnumerable<ResearchDirection>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectResearchDirectionsForAutocompleteQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<ResearchDirection> Handle(SelectResearchDirectionsForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<ResearchDirection> researchDirections;
            if (message.Take != 0)
            {
                researchDirections = _db.FetchBy<ResearchDirection>(f => f.Where(w => w.Title.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                researchDirections = _db.FetchBy<ResearchDirection>(f => f.Where(w => w.Title.Contains(message.Query)));
            }

            return researchDirections;
        }
    }
    public class SelectResearchDirectionsByParentIdQueryHandler : IRequestHandler<SelectResearchDirectionsByParentIdQuery, IEnumerable<ResearchDirection>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectResearchDirectionsByParentIdQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<ResearchDirection> Handle(SelectResearchDirectionsByParentIdQuery message)
        {
            if (message.ParentId == null || message.ParentId == 0) throw new ArgumentNullException($"ParentId is empty: {message.ParentId}");

            IEnumerable<ResearchDirection> researchDirections = _db.FetchBy<ResearchDirection>(f => f.Where(w => w.ParentId == message.ParentId));

            return researchDirections;
        }
    }
}
