using System;
using System.Collections.Generic;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public interface ISagaRepository
    {
        TSaga GetById<TSaga>(string bucketId, string sagaId) where TSaga : class, ISaga;
        void Save(string bucketId, ISaga saga, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
    }
}
