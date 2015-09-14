using SciVacancies.Domain.Enums;
using SciVacancies.WebApp.Infrastructure.Saga;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Quartz;

namespace SciVacancies.WebApp.Infrastructure
{
    public class VacancySagaTimeoutJob : IJob
    {
        public Guid SagaGuid;

        readonly ISagaRepository repository;
        readonly ISchedulerService scheduler;

        public VacancySagaTimeoutJob()
        {
            //оставляем пустым
        }

        public VacancySagaTimeoutJob(ISagaRepository repository, ISchedulerService scheduler)
        {
            this.repository = repository;
        }

        public void Execute(IJobExecutionContext context)
        {
            VacancySaga saga = repository.GetById<VacancySaga>("vacancysaga", SagaGuid);
            
            switch (saga.State)
            {
                case VacancyStatus.Published:
                    if (saga.InCommitteeDate < DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaSwitchedInCommittee());
                        repository.Save("vacancysaga", saga, Guid.NewGuid(), null);
                    }
                    break;

                case VacancyStatus.InCommittee:
                    if (saga.OfferResponseAwaitingDate < DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaSwitchedInOfferAwaiting());
                        repository.Save("vacancysaga", saga, Guid.NewGuid(), null);
                    }
                    break;

                case VacancyStatus.OfferResponseAwaiting:

                    break;

                    //case VacancyStatus.
            }

            if (saga.State == VacancyStatus.Closed || saga.State == VacancyStatus.Cancelled)
            {
                scheduler.RemoveScheduledJob(this.SagaGuid);
            }
        }
    }
}
