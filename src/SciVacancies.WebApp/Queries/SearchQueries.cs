using SciVacancies.ReadModel.ElasticSearchModel.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        //TODO - сделать массивы соответствующих типов
        public IEnumerable<string> Regions { get; set; }
        public IEnumerable<string> Foivs { get; set; }
        public IEnumerable<string> ResearchDirections { get; set; }
        public IEnumerable<string> PositionsTypes { get; set; }
    }
}
