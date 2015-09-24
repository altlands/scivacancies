using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.WebApp.Infrastructure.Saga;
using SciVacancies.WebApp.Commands;
using SciVacancies.SmtpNotifications.SmtpNotificators;
using SciVacancies.WebApp.Queries;
using SciVacancies.ReadModel.Core;

using System;

using MediatR;
using Quartz;
using SciVacancies.Services.Quartz;
using Microsoft.Framework.OptionsModel;

namespace SciVacancies.WebApp.Infrastructure
{
    //TODO убрать репозиторий агрегатов. Нужно через transition нужные команды дёргать у агрегатов
    public class VacancySagaTimeoutJob : IJob
    {
        public Guid SagaGuid;

        /// <summary>
        /// репозиторий с сагами
        /// </summary>
        readonly ISagaRepository sagaRepository;

        readonly IMediator mediator;

        readonly ISchedulerService scheduler;

        readonly IOptions<SagaSettings> settings;

        readonly ISmtpNotificatorVacancyService smtpService;

        public VacancySagaTimeoutJob()
        {
            //оставляем пустым
        }

        public VacancySagaTimeoutJob(ISagaRepository sagaRepository, IMediator mediator, ISchedulerService scheduler, IOptions<SagaSettings> settings, ISmtpNotificatorVacancyService smtpService)
        {
            this.sagaRepository = sagaRepository;
            this.mediator = mediator;
            this.scheduler = scheduler;
            this.settings = settings;
            this.smtpService = smtpService;
        }

        public void Execute(IJobExecutionContext context)
        {
            VacancySaga saga = sagaRepository.GetById<VacancySaga>("vacancysaga", SagaGuid);

            switch (saga.State)
            {
                case VacancyStatus.Published:
                    if (saga.PublishEndDate <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaSwitchedInCommittee
                        {
                            SagaGuid = saga.Id,
                            VacancyGuid = saga.VacancyGuid,
                            OrganizationGuid = saga.OrganizationGuid
                        });
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        mediator.Send(new SwitchVacancyInCommitteeCommand
                        {
                            VacancyGuid = SagaGuid
                        });
                    }
                    break;

                case VacancyStatus.InCommittee:
                    if (!saga.FirstInCommitteeNotificationSent && saga.InCommitteeEndDate.AddDays(settings.Options.Date.Committee.FirstNotificationDays) <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaFirstInCommitteeNotificationSent
                        {
                            SagaGuid = saga.Id,
                            VacancyGuid = saga.VacancyGuid,
                            OrganizationGuid = saga.OrganizationGuid
                        });
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
                    }
                    if (saga.InCommitteeEndDate.AddDays(settings.Options.Date.Committee.SecondNotificationDays) <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaSecondInCommitteeNotificationSent
                        {
                            SagaGuid = saga.Id,
                            VacancyGuid = saga.VacancyGuid,
                            OrganizationGuid = saga.OrganizationGuid
                        });
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        //отправили два увеодмления, теперь ничего не делаем и просто ждём, когда организация выберет победителя и претендта(необязательно) и приложит
                        //документ с рейтингом заявок
                        if (!scheduler.DeleteJob(new JobKey(saga.Id.ToString(), "VacancySagaTimeoutJob")))
                        {
                            throw new Exception("Can't delete job from DB");
                        }
                    }

                    break;

                case VacancyStatus.OfferResponseAwaitingFromWinner:
                    //высылаем победителю уведомление, что пора бы принять решение по предложению контракта
                    if (!saga.OfferResponseNotificationSentToWinner && saga.OfferResponseAwaitingFromWinnerEndDate.AddDays(settings.Options.Date.OfferResponseAwaiting.WinnerNotificationDays) <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaOfferResponseNotificationSentToWinner
                        {
                            SagaGuid = saga.Id,
                            VacancyGuid = saga.VacancyGuid,
                            OrganizationGuid = saga.OrganizationGuid,

                            WinnerReasearcherGuid = saga.WinnerResearcherGuid,
                            WinnerVacancyApplicationGuid = saga.WinnerVacancyApplicationGuid
                        });

                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
                    }
                    if (saga.OfferResponseAwaitingFromWinnerEndDate <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaOfferRejectedByWinner
                        {
                            SagaGuid = saga.Id,
                            VacancyGuid = saga.VacancyGuid,
                            OrganizationGuid = saga.OrganizationGuid
                        });
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);


                        mediator.Send(new SetWinnerRejectOfferCommand
                        {
                            VacancyGuid = SagaGuid
                        });

                        //перевели вакансию в статус "предложение контракта отклонено победителем" и ждём решения от организации (отменять вакансию или отправить оффер претенднету)
                        if (!scheduler.DeleteJob(new JobKey(saga.Id.ToString(), "VacancySagaTimeoutJob")))
                        {
                            throw new Exception("Can't delete job from DB");
                        }
                    }

                    break;

                case VacancyStatus.OfferResponseAwaitingFromPretender:
                    //высылаем претенденту уведомление, что пора бы принять решение по предложению контракта
                    if (!saga.OfferResponseNotificationSentToPretender && saga.OfferResponseAwaitingFromPretenderEndDate.AddDays(settings.Options.Date.OfferResponseAwaiting.PretenderNotificationDays) <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaOfferResponseNotificationSentToPretender
                        {
                            SagaGuid = saga.Id,
                            VacancyGuid = saga.VacancyGuid,
                            OrganizationGuid = saga.OrganizationGuid,

                            PretenderReasearcherGuid = saga.PretenderResearcherGuid,
                            PretenderVacancyApplicationGuid = saga.PretenderVacancyApplicationGuid
                        });
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);
                    }
                    if (saga.OfferResponseAwaitingFromPretenderEndDate <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaOfferRejectedByPretender
                        {
                            SagaGuid = saga.Id,
                            VacancyGuid = saga.VacancyGuid,
                            OrganizationGuid = saga.OrganizationGuid
                        });
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        mediator.Send(new SetPretenderRejectOfferCommand
                        {
                            VacancyGuid = SagaGuid
                        });

                        saga.Transition(new VacancySagaCancelled
                        {
                            SagaGuid = saga.Id,
                            VacancyGuid = saga.VacancyGuid,
                            OrganizationGuid = saga.OrganizationGuid
                        });
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        mediator.Send(new CancelVacancyCommand
                        {
                            VacancyGuid = SagaGuid,
                            Reason = "Pretender didn't click the button"
                        });

                        //перевели вакансию в статус "предложение контракта отклонено претендентом" и ждём решения от организации
                        if (!scheduler.DeleteJob(new JobKey(saga.Id.ToString(), "VacancySagaTimeoutJob")))
                        {
                            throw new Exception("Can't delete job from DB");
                        }
                    }
                    break;

                default:
                    //Если приняли офер или отказались от него, то (если отказался победитель) - ждём решения организации, отправлять ли оффер претенденту
                    if (!scheduler.DeleteJob(new JobKey(saga.Id.ToString(), "VacancySagaTimeoutJob")))
                    {
                        throw new Exception("Can't delete job from DB");
                    }

                    break;
            }
        }
    }
}
