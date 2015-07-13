﻿using SciVacancies.ReadModel.ElasticSearchModel.Model;

using System.Collections.Generic;

using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Queries
{
    public class SearchQuery : IRequest<Page<Vacancy>>
    {
        public string Query { get; set; }

        public long PageSize { get; set; }
        public long CurrentPage { get; set; }
        public string OrderBy { get; set; }

        public IEnumerable<int> FoivIds { get; set; }
        public IEnumerable<int> PositionsTypeIds { get; set; }
        public IEnumerable<int> RegionIds { get; set; }
        public IEnumerable<int> ResearchDirectionIds { get; set; }
    }
}