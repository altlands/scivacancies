using SciVacancies.Domain.Events;

using System;

using MediatR;


namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySagaActivator :
    INotificationHandler<VacancyPublished>,
    INotificationHandler<VacancyInOfferResponseAwaitingFromWinner>,
    INotificationHandler<VacancyInOfferResponseAwaitingFromPretender>
    {
        readonly ISagaRepository sagaRepository;
        readonly ISchedulerService schedulerService;

        public VacancySagaActivator(ISagaRepository sagaRepository, ISchedulerService schedulerService)
        {
            this.sagaRepository = sagaRepository;
            this.schedulerService = schedulerService;
        }
        public void Handle(VacancyPublished msg)
        {
            VacancySaga saga = new VacancySaga(msg.VacancyGuid);
            saga.Transition(new VacancySagaCreated
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,
                //TODO вынести сроки в конфиг
                PublishEndDate = msg.TimeStamp.AddDays(20)
            });

            sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

            var job = new VacancySagaTimeoutJob()
            {
                SagaGuid = saga.Id
            };

            //вынести интервал в конфиг
            schedulerService.CreateSheduledJob(job, job.SagaGuid, 1);
        }
        /// <summary>
        /// Как только у вакансии этот статус проставляется - ожидается респонс от победителя или претендента. Нужно запустить таймер на 30 дней
        /// </summary>
        /// <param name="msg"></param>
        public void Handle(VacancyInOfferResponseAwaitingFromWinner msg)
        {
            throw new NotImplementedException();

            //VacancySaga saga = sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            //saga.Transition(new VacancySagaSwitchedInOfferAwaiting
            //{
            //    //TODO вынести сроки в конфиг
            //    OfferResponseAwaitingEndDate = msg.TimeStamp.AddDays(30)
            //});

            //var job = new VacancySagaTimeoutJob()
            //{
            //    SagaGuid = saga.Id
            //};

            ////вынести интервал в конфиг
            //schedulerService.CreateSheduledJob(job, job.SagaGuid, 1);
        }
        public void Handle(VacancyInOfferResponseAwaitingFromPretender msg)
        {
            throw new NotImplementedException();


        }
    }
}
