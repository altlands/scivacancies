using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Nest;

namespace SciVacancies.ReadModel
{
    public interface IElasticService
    {
        ElasticClient Connect();
        void CreateIndex();
        void RemoveIndex();
        void RestoreIndexFromReadModel();
        void IndexOrganization(Organization organization);
        void UpdateOrganization(Organization organization);
        void IndexVacancy(Vacancy vacancy);
        void UpdateVacancy(Vacancy vacancy);
        ISearchResponse<Vacancy> Search(string query, int pageSize, int pageIndex, List<Guid> regions, List<Guid> foivs, List<Guid> universities, List<int> directions);
    }
}
