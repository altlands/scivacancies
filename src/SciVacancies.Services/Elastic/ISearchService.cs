using SciVacancies.ReadModel.ElasticSearchModel.Model;

using Nest;

namespace SciVacancies.Services.Elastic
{
    public interface ISearchService
    {
        ISearchResponse<Vacancy> VacancySearch(SearchQuery sq);
    }
}