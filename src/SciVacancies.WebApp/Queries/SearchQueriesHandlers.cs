using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Nest;
using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Queries
{
    public class SearchQueryHandler : IRequestHandler<SearchQuery, Page<Vacancy>>
    {
        private readonly IElasticClient _elastic;

        public SearchQueryHandler(IElasticClient elastic)
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

//public ISearchResponse<Vacancy> Search(string query, int pageSize, int pageIndex, List<Guid> regions, List<Guid> foivs, List<Guid> universities, List<int> directions)
//{
//    //                return Connect().Search<ElasticResource>(s => s.Index(DefaultName).Sort(sd => sd.OnField(of => of.Resource.Code.Suffix("raw")).NestedMax().Descending()).Skip((c.Page - 1) * ResPerPage).Take(ResPerPage).MinScore(MinScore).Query(QueryToggle(c, employeeNumber)));
//    return Connect().Search<Vacancy>(s => s
//         .Index(DefaultIndexName)
//         .Skip((pageIndex - 1) * pageSize)
//         .Take(pageSize)
//         .Query(qr => qr
//             .FuzzyLikeThis(flt => flt
//                 .LikeText(query)
//             )
//         //.Filtered(ftd => ftd
//         //    .Query(q => q
//         //        .FuzzyLikeThis(flt => flt.LikeText(query))
//         //    )
//         //    .Filter(f => f
//         //        .Bool(b=>b
//         //            //.Must(mst=>mst
//         //            //    .Terms().
//         //            //)
//         //            //&& b.Must(mst=>mst)
//         //        )                        
//         //    )
//         //)
//         )
//     );
//}