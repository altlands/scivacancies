using SciVacancies.ReadModel.ElasticSearchModel.Model;

using System.Linq;

using Nest;
using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Queries
{
    public class SearchQueriesHandler : IRequestHandler<SearchQuery, Page<Vacancy>>
    {
        private readonly IElasticClient _elastic;

        public SearchQueriesHandler(IElasticClient elastic)
        {
            _elastic = elastic;
        }

        public Page<Vacancy> Handle(SearchQuery message)
        {

            var results = _elastic.Search<Vacancy>(s => s
                                    .Index("scivacancies")
                                    .Skip((int)((message.PageIndex - 1) * message.PageSize))
                                    .Take((int)message.PageSize)
                                    .Query(qr => qr
                                        .FuzzyLikeThis(flt => flt
                                            .LikeText(message.Query))
                                    )
                                    //.Query(qr => qr
                                    //    .Filtered(fltd => fltd
                                    //        .Query(q => q
                                    //            .FuzzyLikeThis(flt => flt
                                    //                .LikeText(message.Query)
                                    //            )
                                    //        )
                                    //        .Filter(f => f
                                    //        //TODO - сделать массивы гуидов а не стринг
                                    //            .Terms("positionTypeGuid", message.PositionsTypes)
                                    //        && f.Terms("researchDirectionId", message.ResearchDirections)
                                    //        && f.Terms("regionId", message.Regions)
                                    //        && f.Terms("foivId", message.Foivs)
                                    //        )
                                    //    )
                                    //)
                                    );
            var pageVacancies = new Page<Vacancy>()
            {
                CurrentPage = message.PageIndex,
                ItemsPerPage = message.PageSize,
                TotalItems = results.Total,
                TotalPages = results.Total / message.PageSize,
                Items = results.Documents.ToList()
            };

            return pageVacancies;
        }
    }
}