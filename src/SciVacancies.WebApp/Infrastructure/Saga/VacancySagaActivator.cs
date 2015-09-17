using SciVacancies.Domain.Events;

using System;

using MediatR;
using SciVacancies.Services.Quartz;
using Microsoft.Framework.OptionsModel;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySagaActivator :
    INotificationHandler<VacancyPublished>,
    INotificationHandler<VacancyProlongedInCommittee>,
    INotificationHandler<VacancyInOfferResponseAwaitingFromWinner>,
    INotificationHandler<VacancyOfferAcceptedByWinner>,
    INotificationHandler<VacancyOfferRejectedByWinner>,
    INotificationHandler<VacancyInOfferResponseAwaitingFromPretender>,
    INotificationHandler<VacancyOfferAcceptedByPretender>,
    INotificationHandler<VacancyOfferRejectedByPretender>,
    INotificationHandler<VacancyCancelled>,
    INotificationHandler<VacancyClosed>
    {
        readonly ISagaRepository sagaRepository;
        readonly ISchedulerService schedulerService;
        readonly IOptions<QuartzSettings> settings;

        public VacancySagaActivator(ISagaRepository sagaRepository, ISchedulerService schedulerService, IOptions<QuartzSettings> settings)
        {
            this.sagaRepository = sagaRepository;
            this.schedulerService = schedulerService;
            this.settings = settings;
        }
        public void Handle(VacancyPublished msg)
        {
            VacancySaga saga = new VacancySaga(msg.VacancyGuid);
            saga.Transition(new VacancySagaCreated
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,

                InCommitteeStartDate = msg.InCommitteeStartDate,
                InCommitteeEndDate = msg.InCommitteeEndDate
            });

            sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

            var job = new VacancySagaTimeoutJob()
            {
                SagaGuid = saga.Id
            };

            //вынести интервал в конфиг
            schedulerService.CreateSheduledJob(job, job.SagaGuid, settings.Options.Scheduler.ExecutionInterval);
        }

        public void Handle(VacancyProlongedInCommittee msg)
        {
            VacancySaga saga = sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaProlongedInCommittee
            {
                SagaGuid = saga.Id,

                InCommitteeEndDate = msg.InCommitteeEndDate
            });

            sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }

        /// <summary>
        /// Как только у вакансии этот статус проставляется - ожидается респонс от победителя или претендента. Нужно запустить таймер на 30 дней
        /// </summary>
        /// <param name="msg"></param>
        public void Handle(VacancyInOfferResponseAwaitingFromWinner msg)
        {
            VacancySaga saga = sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaSwitchedInOfferResponseAwaitingFromWinner
            {
                SagaGuid = saga.Id,
                OfferResponseAwaitingFromWinnerEndDate = msg.TimeStamp.AddDays(30)
            });

            sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

            var job = new VacancySagaTimeoutJob()
            {
                SagaGuid = saga.Id
            };

            //вынести интервал в конфиг
            schedulerService.CreateSheduledJob(job, job.SagaGuid, settings.Options.Scheduler.ExecutionInterval);
        }
        public void Handle(VacancyOfferAcceptedByWinner msg)
        {
            VacancySaga saga = sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaOfferAcceptedByWinner
            {
                SagaGuid = saga.Id
            });

            sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
        public void Handle(VacancyOfferRejectedByWinner msg)
        {
            VacancySaga saga = sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaOfferRejectedByWinner
            {
                SagaGuid = saga.Id
            });

            sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
        public void Handle(VacancyInOfferResponseAwaitingFromPretender msg)
        {
            VacancySaga saga = sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaSwitchedInOfferResponseAwaitingFromPretender
            {
                SagaGuid = saga.Id,
                OfferResponseAwaitingFromPretenderEndDate = msg.TimeStamp.AddDays(30)
            });

            sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

            var job = new VacancySagaTimeoutJob()
            {
                SagaGuid = saga.Id
            };

            //вынести интервал в конфиг
            schedulerService.CreateSheduledJob(job, job.SagaGuid, settings.Options.Scheduler.ExecutionInterval);
        }
        public void Handle(VacancyOfferAcceptedByPretender msg)
        {
            VacancySaga saga = sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaOfferAcceptedByPretender
            {
                SagaGuid = saga.Id
            });

            sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
        public void Handle(VacancyOfferRejectedByPretender msg)
        {
            VacancySaga saga = sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaOfferRejectedByPretender
            {
                SagaGuid = saga.Id
            });

            sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
        public void Handle(VacancyCancelled msg)
        {
            VacancySaga saga = sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaCancelled
            {
                SagaGuid = saga.Id
            });

            sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
        public void Handle(VacancyClosed msg)
        {
            VacancySaga saga = sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaClosed
            {
                SagaGuid = saga.Id
            });

            sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
    }
}
