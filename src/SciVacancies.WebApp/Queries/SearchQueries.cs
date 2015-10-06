using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.ElasticSearchModel.Model;

using System.Collections.Generic;
using System;

using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Queries
{
    public class SearchQuery : SciVacancies.Services.Elastic.SearchQuery, IRequest<Page<Vacancy>>
    {
        //public string Query { get; set; }

        //public long? PageSize { get; set; }
        //public long? CurrentPage { get; set; }

        //public string OrderFieldByDirection { get; set; }

        //public DateTime? PublishDateFrom { get; set; }

        //public IEnumerable<int> FoivIds { get; set; }
        //public IEnumerable<int> PositionTypeIds { get; set; }
        //public IEnumerable<int> RegionIds { get; set; }
        //public IEnumerable<int> ResearchDirectionIds { get; set; }

        //public int? SalaryFrom { get; set; }
        //public int? SalaryTo { get; set; }
        //public IEnumerable<VacancyStatus> VacancyStatuses { get; set; }
    }
}
