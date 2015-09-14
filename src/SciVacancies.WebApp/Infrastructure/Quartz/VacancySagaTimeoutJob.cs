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

        readonly ISagaRepository sagaRepository;
        readonly ISchedulerService scheduler;

        public VacancySagaTimeoutJob()
        {
            //оставляем пустым
        }

        public VacancySagaTimeoutJob(ISagaRepository sagaRepository, ISchedulerService scheduler)
        {
            this.sagaRepository = sagaRepository;
        }

        public void Execute(IJobExecutionContext context)
        {
            VacancySaga saga = sagaRepository.GetById<VacancySaga>("vacancysaga", SagaGuid);
            
            switch (saga.State)
            {
                case VacancyStatus.Published:
                    if (saga.InCommitteeDate <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaSwitchedInCommittee());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                    }
                    break;

                case VacancyStatus.InCommittee:
                    if (saga.OfferResponseAwaitingDate <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaSwitchedInOfferAwaiting());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
                    }
                    break;

                case VacancyStatus.OfferResponseAwaiting:

                    break;

                case VacancyStatus.OfferAccepted:

                    break;

                case VacancyStatus.OfferRejected:

                    break;
            }

            if (saga.State == VacancyStatus.Closed || saga.State == VacancyStatus.Cancelled)
            {
                scheduler.RemoveScheduledJob(SagaGuid);
            }
        }
    }
}
