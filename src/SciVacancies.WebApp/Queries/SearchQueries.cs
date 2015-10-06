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

    }
}
