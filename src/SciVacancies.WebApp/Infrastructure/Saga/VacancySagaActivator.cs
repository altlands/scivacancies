﻿using SciVacancies.Domain.Events;

using System;

using Quartz;
using MediatR;
using SciVacancies.Services.Quartz;
using Microsoft.Framework.OptionsModel;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySagaActivator :
    INotificationHandler<VacancyPublished>,
    INotificationHandler<VacancyProlongedInCommittee>,
    INotificationHandler<VacancyWinnerSet>,
    INotificationHandler<VacancyPretenderSet>,
    INotificationHandler<VacancyInOfferResponseAwaitingFromWinner>,
    INotificationHandler<VacancyOfferAcceptedByWinner>,
    INotificationHandler<VacancyOfferRejectedByWinner>,
    INotificationHandler<VacancyInOfferResponseAwaitingFromPretender>,
    INotificationHandler<VacancyOfferAcceptedByPretender>,
    INotificationHandler<VacancyOfferRejectedByPretender>,
    INotificationHandler<VacancyCancelled>,
    INotificationHandler<VacancyClosed>
    {
        readonly ISagaRepository _sagaRepository;
        readonly ISchedulerService _schedulerService;
        readonly IOptions<QuartzSettings> _settings;

        public VacancySagaActivator(ISagaRepository sagaRepository, ISchedulerService schedulerService, IOptions<QuartzSettings> settings)
        {
            _sagaRepository = sagaRepository;
            _schedulerService = schedulerService;
            _settings = settings;
        }
        public void Handle(VacancyPublished msg)
        {
            var saga = new VacancySaga(msg.VacancyGuid);
            saga.Transition(new VacancySagaCreated
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,

                InCommitteeStartDate = msg.InCommitteeStartDate,
                InCommitteeEndDate = msg.InCommitteeEndDate
            });

            _sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

            var job = new VacancySagaTimeoutJob()
            {
                SagaGuid = saga.Id
            };

            _schedulerService.CreateSheduledJobWithStrongName(job, new JobKey(job.SagaGuid.ToString(), "VacancySagaTimeoutJob"), _settings.Value.Scheduler.ExecutionInterval);
        }

        public void Handle(VacancyProlongedInCommittee msg)
        {
            var saga = _sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaProlongedInCommittee
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,

                InCommitteeEndDate = msg.InCommitteeEndDate
            });

            _sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }

        public void Handle(VacancyWinnerSet msg)
        {
            VacancySaga saga = _sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaWinnerSet
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,

                WinnerReasearcherGuid = msg.WinnerReasearcherGuid,
                WinnerVacancyApplicationGuid = msg.WinnerVacancyApplicationGuid
            });

            _sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
        public void Handle(VacancyPretenderSet msg)
        {
            VacancySaga saga = _sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaPretenderSet
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,

                PretenderReasearcherGuid = msg.PretenderReasearcherGuid,
                PretenderVacancyApplicationGuid = msg.PretenderVacancyApplicationGuid
            });

            _sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
        /// <summary>
        /// Как только у вакансии этот статус проставляется - ожидается респонс от победителя или претендента. Нужно запустить таймер на 30 дней
        /// </summary>
        /// <param name="msg"></param>
        public void Handle(VacancyInOfferResponseAwaitingFromWinner msg)
        {
            VacancySaga saga = _sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaSwitchedInOfferResponseAwaitingFromWinner
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,

                OfferResponseAwaitingFromWinnerEndDate = msg.TimeStamp.AddDays(30)
            });

            _sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

            var job = new VacancySagaTimeoutJob()
            {
                SagaGuid = saga.Id
            };

            if (_schedulerService.CheckExists(new JobKey(saga.Id.ToString(), "VacancySagaTimeoutJob")))
            {
                if (!_schedulerService.DeleteJob(new JobKey(saga.Id.ToString(), "VacancySagaTimeoutJob")))
                {
                    throw new Exception("Can't delete job from DB");
                }
            }

            _schedulerService.CreateSheduledJobWithStrongName(job, new JobKey(job.SagaGuid.ToString(), "VacancySagaTimeoutJob"), _settings.Value.Scheduler.ExecutionInterval);
        }
        public void Handle(VacancyOfferAcceptedByWinner msg)
        {
            VacancySaga saga = _sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaOfferAcceptedByWinner
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,
            });

            _sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
        public void Handle(VacancyOfferRejectedByWinner msg)
        {
            VacancySaga saga = _sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaOfferRejectedByWinner
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,
            });

            _sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
        public void Handle(VacancyInOfferResponseAwaitingFromPretender msg)
        {
            VacancySaga saga = _sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaSwitchedInOfferResponseAwaitingFromPretender
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,

                OfferResponseAwaitingFromPretenderEndDate = msg.TimeStamp.AddDays(30)
            });

            _sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

            var job = new VacancySagaTimeoutJob()
            {
                SagaGuid = saga.Id
            };

            if (_schedulerService.CheckExists(new JobKey(saga.Id.ToString(), "VacancySagaTimeoutJob")))
            {
                if (!_schedulerService.DeleteJob(new JobKey(saga.Id.ToString(), "VacancySagaTimeoutJob")))
                {
                    throw new Exception("Can't delete job from DB");
                }
            }

            _schedulerService.CreateSheduledJobWithStrongName(job, new JobKey(job.SagaGuid.ToString(), "VacancySagaTimeoutJob"), _settings.Value.Scheduler.ExecutionInterval);
        }
        public void Handle(VacancyOfferAcceptedByPretender msg)
        {
            VacancySaga saga = _sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaOfferAcceptedByPretender
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,
            });

            _sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
        public void Handle(VacancyOfferRejectedByPretender msg)
        {
            VacancySaga saga = _sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaOfferRejectedByPretender
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,
            });

            _sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
        public void Handle(VacancyCancelled msg)
        {
            VacancySaga saga = _sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaCancelled
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,
            });

            _sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
        public void Handle(VacancyClosed msg)
        {
            VacancySaga saga = _sagaRepository.GetById<VacancySaga>("vacancysaga", msg.VacancyGuid);
            saga.Transition(new VacancySagaClosed
            {
                SagaGuid = saga.Id,
                VacancyGuid = msg.VacancyGuid,
                OrganizationGuid = msg.OrganizationGuid,
            });

            _sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
        }
    }
}
