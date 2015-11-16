using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Caching.Memory;

using MediatR;
using NPoco;

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
        IRequestHandler<SelectRegionsByGuidsQuery, IEnumerable<Region>>,

        IRequestHandler<SelectAllResearchDirectionsQuery, IEnumerable<ResearchDirection>>,
        IRequestHandler<SelectResearchDirectionsForAutocompleteQuery, IEnumerable<ResearchDirection>>,
        IRequestHandler<SelectResearchDirectionsByParentIdQuery, IEnumerable<ResearchDirection>>,
        IRequestHandler<SelectResearchDirectionQuery, ResearchDirection>,

        IRequestHandler<SelectAllAttachmentTypesQuery, IEnumerable<AttachmentType>>
    {
        private readonly IDatabase _db;
        private readonly IMemoryCache cache;
        private readonly IOptions<CacheSettings> cacheSettings;
        private MemoryCacheEntryOptions cacheOptions
        {
            get
            {
                return new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(cacheSettings.Value.DictionaryExpiration));
            }
        }

        public DictionaryQueriesHandler(IDatabase db, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
        {
            _db = db;
            this.cache = cache;
            this.cacheSettings = cacheSettings;
        }

        public IEnumerable<Criteria> Handle(SelectAllCriteriasQuery message)
        {
            IEnumerable<Criteria> criterias;
            if (!cache.TryGetValue<IEnumerable<Criteria>>("d_criterias", out criterias))
            {
                criterias = _db.Fetch<Criteria>();
                cache.Set<IEnumerable<Criteria>>("d_criterias", criterias, cacheOptions);
            }

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
            IEnumerable<Foiv> foivs;
            if (!cache.TryGetValue("d_foivs", out foivs))
            {
                foivs = _db.Fetch<Foiv>();
                cache.Set<IEnumerable<Foiv>>("d_foivs", foivs, cacheOptions);
            }

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
            IEnumerable<OrgForm> orgForms;
            if (!cache.TryGetValue<IEnumerable<OrgForm>>("d_orgforms", out orgForms))
            {
                orgForms = _db.Fetch<OrgForm>();
                cache.Set<IEnumerable<OrgForm>>("d_orgforms", orgForms, cacheOptions);
            }

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
            IEnumerable<PositionType> positionTypes;
            if (!cache.TryGetValue<IEnumerable<PositionType>>("d_positiontypes", out positionTypes))
            {
                positionTypes = _db.Fetch<PositionType>();
                cache.Set<IEnumerable<PositionType>>("d_positiontypes", positionTypes, cacheOptions);
            }

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
            IEnumerable<Region> regions;
            if (!cache.TryGetValue<IEnumerable<Region>>("d_regions", out regions))
            {
                regions = _db.Fetch<Region>();
                cache.Set<IEnumerable<Region>>("d_regions", regions, cacheOptions);
            }

            return regions;
        }

        public IEnumerable<Region> Handle(SelectRegionsByGuidsQuery message)
        {
            IEnumerable<Region> allRegions;
            if (!cache.TryGetValue("d_regions", out allRegions))
            {
                allRegions = _db.Fetch<Region>();
                cache.Set<IEnumerable<Region>>("d_regions", allRegions, cacheOptions);
            }
            var regions = allRegions.Where(w => message.RegionIds.Contains(w.id)).OrderByDescending(o => o.title).ToList();
            //var regions = _db.Fetch<Region>(new Sql($"SELECT d.* FROM d_regions d WHERE d.id IN (@0) ORDER BY d.title DESC", message.RegionIds));

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
            IEnumerable<ResearchDirection> researchDirections;
            if (!cache.TryGetValue<IEnumerable<ResearchDirection>>("d_researchdirections", out researchDirections))
            {
                researchDirections = _db.Fetch<ResearchDirection>();
                cache.Set<IEnumerable<ResearchDirection>>("d_researchdirections", researchDirections, cacheOptions);
            }

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
        public ResearchDirection Handle(SelectResearchDirectionQuery message)
        {
            if (message.Id == 0) throw new ArgumentNullException(nameof(message.Id));
            return _db.SingleById<ResearchDirection>(message.Id);
        }

        public IEnumerable<AttachmentType> Handle(SelectAllAttachmentTypesQuery message)
        {
            IEnumerable<AttachmentType> attachmentTypes;
            if (!cache.TryGetValue<IEnumerable<AttachmentType>>("d_attachmenttypes", out attachmentTypes))
            {
                attachmentTypes = _db.Fetch<AttachmentType>();
                cache.Set<IEnumerable<AttachmentType>>("d_attachmenttypes", attachmentTypes, cacheOptions);
            }

            return attachmentTypes;
        }
    }

}
