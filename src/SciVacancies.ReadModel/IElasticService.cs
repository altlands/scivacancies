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
        void DeleteIndex();
        void RestoreIndexFromReadModel();
        void Search();
    }
}
