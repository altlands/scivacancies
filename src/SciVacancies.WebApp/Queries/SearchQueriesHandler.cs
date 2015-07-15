using SciVacancies.Domain.Enums;
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

        public Page<Vacancy> Handle(SearchQuery msg)
        {
            var results = _elastic.Search<Vacancy>(s => s
                                    .Index("scivacancies")
                                    .Skip((int)((msg.CurrentPage - 1) * (msg.PageSize??10)))
                                    .Take((int)(msg.PageSize ?? 10))
                                    .Query(qr => qr
                                        .Filtered(fltd => fltd
                                            .Query(q => q
                                                .FuzzyLikeThis(flt => flt
                                                    .LikeText(msg.Query)
                                                )
                                            )
                                            .Filter(f => f
                                                .Terms<int>(ft => ft.OrganizationFoivId, msg.FoivIds)
                                                && f.Terms<int>(ft => ft.PositionTypeId, msg.PositionTypeIds)
                                                && f.Terms<int>(ft => ft.RegionId, msg.RegionIds)
                                                && f.Terms<int>(ft => ft.ResearchDirectionId, msg.ResearchDirectionIds)
                                                && f.Range(fr => fr
                                                    .GreaterOrEquals((long)(msg.SalaryFrom ?? 0))
                                                    .LowerOrEquals((long)(msg.SalaryTo ?? 0))
                                                )
                                                && f.Terms<VacancyStatus>(ft => ft.Status, msg.VacancyStatuses)
                                            )
                                        )
                                    )
                                    );
            var pageVacancies = new Page<Vacancy>
            {
                CurrentPage = msg.CurrentPage ?? 1,
                ItemsPerPage = msg.PageSize ?? 10,
                TotalItems = results.Total,
                TotalPages = results.Total / (msg.PageSize ?? 10),
                Items = results.Documents.ToList()
            };

            return pageVacancies;
        }
    }
}