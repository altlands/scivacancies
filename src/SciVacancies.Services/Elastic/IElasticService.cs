using SciVacancies.ReadModel.ElasticSearchModel.Model;

using Nest;

namespace SciVacancies.Services.Elastic
{
    public interface IElasticService
    {
        ISearchResponse<Vacancy> VacancySearch(SearchQuery sq);
        //TODO поиск по организациям
    }
}
