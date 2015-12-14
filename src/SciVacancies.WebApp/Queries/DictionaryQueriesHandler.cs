using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.Logging;

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
        IRequestHandler<SelectRegionsByIdsQuery, IEnumerable<Region>>,

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
        private ILogger Logger;

        private readonly string CriteriasCacheKey = "d_criterias";
        private readonly string FoivsCacheKey = "d_foivs";
        private readonly string OrgFormsCacheKey = "d_orgforms";
        private readonly string PositionTypesCacheKey = "d_positiontypes";
        private readonly string RegionsCacheKey = "d_regions";
        private readonly string ResearchDirectionsCacheKey = "d_researchdirections";
        private readonly string AttachmentTypesCacheKey = "d_attachmenttypes";

        public DictionaryQueriesHandler(IDatabase db, IMemoryCache cache, IOptions<CacheSettings> cacheSettings, ILoggerFactory loggerFactory)
        {
            _db = db;
            this.cache = cache;
            this.cacheSettings = cacheSettings;
            this.Logger = loggerFactory.CreateLogger<DictionaryQueriesHandler>();
        }

        private IEnumerable<Criteria> GetCachedAllCriterias()
        {
            IEnumerable<Criteria> criterias;
            if (!cache.TryGetValue<IEnumerable<Criteria>>(CriteriasCacheKey, out criterias))
            {
                try
                {
                    criterias = _db.Fetch<Criteria>();
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message, e);
                    throw;
                }
                if (criterias != null) cache.Set<IEnumerable<Criteria>>(CriteriasCacheKey, criterias, cacheOptions);
            }

            return criterias;
        }
        public IEnumerable<Criteria> Handle(SelectAllCriteriasQuery message)
        {
            return GetCachedAllCriterias();
        }
        public IEnumerable<Criteria> Handle(SelectCriteriasForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Criteria> criterias;

            if (message.Take != 0)
            {
                criterias = GetCachedAllCriterias()?.Where(w => w.title.Contains(message.Query))?.Take(message.Take)?.ToList();
            }
            else
            {
                criterias = GetCachedAllCriterias()?.Where(w => w.title.Contains(message.Query))?.ToList();
            }

            return criterias;
        }
        public IEnumerable<Criteria> Handle(SelectCriteriasByParentIdQuery message)
        {
            if (message.ParentId == 0) throw new ArgumentNullException($"ParentId is empty or 0: {message.ParentId}");

            IEnumerable<Criteria> criterias = GetCachedAllCriterias()?.Where(w => w.parent_id == message.ParentId)?.ToList();

            return criterias;
        }

        private IEnumerable<Foiv> GetCachedAllFoivs()
        {
            IEnumerable<Foiv> foivs;
            if (!cache.TryGetValue<IEnumerable<Foiv>>(FoivsCacheKey, out foivs))
            {
                try
                {
                    foivs = _db.Fetch<Foiv>();
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message, e);
                    throw;
                }
                if (foivs != null) cache.Set<IEnumerable<Foiv>>(FoivsCacheKey, foivs, cacheOptions);
            }

            return foivs;
        }
        public IEnumerable<Foiv> Handle(SelectAllFoivsQuery message)
        {
            return GetCachedAllFoivs();
        }
        public IEnumerable<Foiv> Handle(SelectFoivsForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Foiv> foivs;

            if (message.Take != 0)
            {
                foivs = GetCachedAllFoivs()?.Where(w => w.title.Contains(message.Query))?.Take(message.Take)?.ToList();
            }
            else
            {
                foivs = GetCachedAllFoivs()?.Where(w => w.title.Contains(message.Query))?.ToList();
            }

            return foivs;
        }
        public IEnumerable<Foiv> Handle(SelectFoivsByParentIdQuery message)
        {
            if (message.ParentId == 0) throw new ArgumentNullException($"ParentId is empty or 0: {message.ParentId}");

            IEnumerable<Foiv> foivs;

            foivs = GetCachedAllFoivs()?.Where(w => w.parent_id == message.ParentId)?.ToList();

            return foivs;
        }

        private IEnumerable<OrgForm> GetCachedAllOrgForms()
        {
            IEnumerable<OrgForm> orgForms;
            if (!cache.TryGetValue(OrgFormsCacheKey, out orgForms))
            {
                try
                {
                    orgForms = _db.Fetch<OrgForm>();
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message, e);
                    throw;
                }
                if (orgForms != null) cache.Set<IEnumerable<OrgForm>>(OrgFormsCacheKey, orgForms, cacheOptions);
            }

            return orgForms;
        }
        public IEnumerable<OrgForm> Handle(SelectAllOrgFormsQuery message)
        {
            return GetCachedAllOrgForms();
        }
        public IEnumerable<OrgForm> Handle(SelectOrgFormsForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<OrgForm> orgForms;

            if (message.Take != 0)
            {
                orgForms = GetCachedAllOrgForms()?.Where(w => w.title.Contains(message.Query))?.Take(message.Take)?.ToList();
            }
            else
            {
                orgForms = GetCachedAllOrgForms()?.Where(w => w.title.Contains(message.Query))?.ToList();
            }

            return orgForms;
        }

        private IEnumerable<PositionType> GetCachedAllPositionTypes()
        {
            IEnumerable<PositionType> positionTypes;
            if (!cache.TryGetValue(PositionTypesCacheKey, out positionTypes))
            {
                try
                {
                    positionTypes = _db.Fetch<PositionType>();
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message, e);
                    throw;
                }
                if (positionTypes != null) cache.Set<IEnumerable<PositionType>>(PositionTypesCacheKey, positionTypes, cacheOptions);
            }

            return positionTypes;
        }
        public IEnumerable<PositionType> Handle(SelectAllPositionTypesQuery message)
        {
            return GetCachedAllPositionTypes();
        }
        public IEnumerable<PositionType> Handle(SelectPositionTypesForAutocompleteQuery message)
        {
            if (string.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<PositionType> positionTypes;

            if (message.Take != 0)
            {
                positionTypes = GetCachedAllPositionTypes()?.Where(w => w.title.Contains(message.Query))?.Take(message.Take)?.ToList();
            }
            else
            {
                positionTypes = GetCachedAllPositionTypes()?.Where(w => w.title.Contains(message.Query))?.ToList();
            }

            return positionTypes;
        }

        private IEnumerable<Region> GetCachedAllRegions()
        {
            IEnumerable<Region> regions;
            if (!cache.TryGetValue(RegionsCacheKey, out regions))
            {
                try
                {
                    regions = _db.Fetch<Region>();
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message, e);
                    throw;
                }
                if (regions != null) cache.Set<IEnumerable<Region>>(RegionsCacheKey, regions, cacheOptions);
            }

            return regions;
        }
        public IEnumerable<Region> Handle(SelectAllRegionsQuery message)
        {
            return GetCachedAllRegions();
        }
        public IEnumerable<Region> Handle(SelectRegionsByIdsQuery message)
        {
            if (message.RegionIds == null || message.RegionIds.Count == 0) throw new ArgumentNullException($"RegionIds is null or empty: {message.RegionIds}");

            IEnumerable<Region> regions;

            regions = GetCachedAllRegions()?.Where(w => message.RegionIds.Contains(w.id))?.OrderByDescending(o => o.title).ToList();

            return regions;
        }
        public IEnumerable<Region> Handle(SelectRegionsForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Region> regions;

            if (message.Take != 0)
            {
                regions = GetCachedAllRegions()?.Where(w => w.title.Contains(message.Query))?.Take(message.Take)?.ToList();
            }
            else
            {
                regions = GetCachedAllRegions()?.Where(w => w.title.Contains(message.Query))?.ToList();
            }

            return regions;
        }

        private IEnumerable<ResearchDirection> GetCachedAllResearchDirections()
        {
            IEnumerable<ResearchDirection> researchDirections;
            if (!cache.TryGetValue(ResearchDirectionsCacheKey, out researchDirections))
            {
                try
                {
                    researchDirections = _db.Fetch<ResearchDirection>();
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message, e);
                    throw;
                }
                if (researchDirections != null) cache.Set<IEnumerable<ResearchDirection>>(ResearchDirectionsCacheKey, researchDirections, cacheOptions);
            }

            return researchDirections;
        }
        public IEnumerable<ResearchDirection> Handle(SelectAllResearchDirectionsQuery message)
        {
            return GetCachedAllResearchDirections();
        }
        public IEnumerable<ResearchDirection> Handle(SelectResearchDirectionsForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<ResearchDirection> researchDirections;

            if (message.Take != 0)
            {
                researchDirections = GetCachedAllResearchDirections()?.Where(w => w.title.Contains(message.Query))?.Take(message.Take)?.ToList();
            }
            else
            {
                researchDirections = GetCachedAllResearchDirections()?.Where(w => w.title.Contains(message.Query))?.ToList();
            }

            return researchDirections;
        }
        public IEnumerable<ResearchDirection> Handle(SelectResearchDirectionsByParentIdQuery message)
        {
            if (message.ParentId == 0) throw new ArgumentNullException($"ParentId is empty or 0: {message.ParentId}");

            IEnumerable<ResearchDirection> researchDirections;

            researchDirections = GetCachedAllResearchDirections()?.Where(w => w.parent_id == message.ParentId)?.ToList();

            return researchDirections;
        }
        public ResearchDirection Handle(SelectResearchDirectionQuery message)
        {
            if (!message.Id.HasValue
                || message.Id.Value == 0)
                return null;//throw new ArgumentNullException(nameof(message.Id));

            var researchDirection = GetCachedAllResearchDirections()?.First(f => f.id == message.Id);
            return researchDirection;
        }

        private IEnumerable<AttachmentType> GetCachedAllAttachmentTypes()
        {
            IEnumerable<AttachmentType> attachmentTypes;
            if (!cache.TryGetValue(AttachmentTypesCacheKey, out attachmentTypes))
            {
                try
                {
                    attachmentTypes = _db.Fetch<AttachmentType>();
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message, e);
                    throw;
                }
                if (attachmentTypes != null) cache.Set<IEnumerable<AttachmentType>>(AttachmentTypesCacheKey, attachmentTypes, cacheOptions);
            }

            return attachmentTypes;
        }
        public IEnumerable<AttachmentType> Handle(SelectAllAttachmentTypesQuery message)
        {
            return GetCachedAllAttachmentTypes();
        }
    }

}
