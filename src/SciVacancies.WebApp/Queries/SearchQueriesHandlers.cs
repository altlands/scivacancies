using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Nest;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SearchQueryHandler : IRequestHandler<SearchQuery, string>
    {
        private readonly IElasticClient _elastic;

        public SearchQueryHandler(IElasticClient elastic)
        {
            _elastic = elastic;
        }

        public string Handle(SearchQuery message)
        {
            var results = _elastic.Search<Vacancy>(s => s
                                    .Index("scivacancies")
                                    .Skip((int)((message.PageIndex - 1) * message.PageSize))
                                    .Take((int)message.PageSize)
                                    .Query(qr => qr
                                        .FuzzyLikeThis(flt => flt
                                            .LikeText(message.Query))
                                    )
                                    );

            return "";
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