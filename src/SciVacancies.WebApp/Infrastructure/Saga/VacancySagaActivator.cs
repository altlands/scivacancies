using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SciVacancies.Domain.Events;
using CommonDomain;
using CommonDomain.Persistence;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySagaActivator :
        INotificationHandler<VacancyPublished>
    {
        readonly ISagaRepository _repository;
        readonly ISchedulerService _schedulerService;

        public VacancySagaActivator(ISagaRepository repository, ISchedulerService schedulerService)
        {
            _repository = repository;
            _schedulerService = schedulerService;
        }
        public void Handle(VacancyPublished msg)
        {
            //VacancySaga saga = new VacancySaga();

            //saga.Transition(new VacancySagaCreated { SagaGuid = Guid.NewGuid() });
            //saga.Transition(new VacancySagaSwitchedInCommittee { SagaGuid = Guid.NewGuid() });

            //_repository.Save("saga", saga, Guid.NewGuid(), null);

            //var s = _repository.GetById<VacancySaga>("saga", saga.Id);

            ////s.Transition<VacancySagaSwitchedInCommittee>(new VacancySagaSwitchedInCommittee { SagaGuid = Guid.NewGuid() });

            //_repository.Save("saga", s, Guid.NewGuid(), null);

            ////var d = _repository.GetById<VacancySaga>("saga", s.Id);
        }
    }
}
