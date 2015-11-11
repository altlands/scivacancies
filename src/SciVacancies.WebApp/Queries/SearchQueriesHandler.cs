using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.ElasticSearchModel.Model;
using SciVacancies.Services.Elastic;

using System;
using System.Collections.Generic;
using System.Linq;

using Nest;
using MediatR;
using NPoco;
using Microsoft.Framework.OptionsModel;

namespace SciVacancies.WebApp.Queries
{
    public class SearchQueriesHandler : IRequestHandler<SearchQuery, Page<Vacancy>>
    {
        readonly ISearchService elasticService;

        public SearchQueriesHandler(ISearchService elasticService)
        {
            this.elasticService = elasticService;
        }

        public Page<Vacancy> Handle(SearchQuery msg)
        {
            var results = elasticService.VacancySearch(msg);

            var pageVacancies = new Page<Vacancy>
            {
                CurrentPage = msg.CurrentPage.Value,
                ItemsPerPage = msg.PageSize.Value,
                TotalItems = results.Total,
                TotalPages = msg.PageSize.HasValue ? results.Total / msg.PageSize.Value : 0,
                Items = results.Documents.ToList()
            };

            return pageVacancies;
        }
    }
}