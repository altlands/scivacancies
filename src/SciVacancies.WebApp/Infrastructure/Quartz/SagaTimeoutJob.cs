using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SciVacancies.WebApp.Infrastructure.Saga;
using Quartz;

namespace SciVacancies.WebApp.Infrastructure
{
    public class VacancySagaTimeoutJob : IJob
    {
        public Guid SagaGuid;

        readonly ISagaRepository repository;

        public VacancySagaTimeoutJob(ISagaRepository repository)
        {
            this.repository = repository;
        }

        public VacancySagaTimeoutJob()
        {

        }

        public void Execute(IJobExecutionContext context)
        {
            VacancySaga saga = repository.GetById<VacancySaga>("saga", SagaGuid);
        }
    }
}
