using SciVacancies.Domain.Events;

using System;

using MediatR;


namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySagaActivator : INotificationHandler<VacancyPublished>
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
            VacancySaga saga = new VacancySaga(msg.VacancyGuid);
            saga.Transition(new VacancySagaCreated
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid
            });

            _repository.Save("vacancysaga", saga, Guid.NewGuid(), null);

            var job = new VacancySagaTimeoutJob()
            {
                SagaGuid = saga.Id
            };

            _schedulerService.CreateSheduledJob(job, job.SagaGuid, 1);
        }
    }
}
