using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Aggregates;
using SciVacancies.WebApp.Infrastructure.Saga;
using SciVacancies.WebApp.Commands;

using System;

using MediatR;
using Quartz;
using SciVacancies.Services.Quartz;

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

        public VacancySagaTimeoutJob()
        {
            //оставляем пустым
        }

        public VacancySagaTimeoutJob(ISagaRepository sagaRepository, IMediator mediator, ISchedulerService scheduler)
        {
            this.sagaRepository = sagaRepository;
            this.scheduler = scheduler;
        }

        public void Execute(IJobExecutionContext context)
        {
            VacancySaga saga = sagaRepository.GetById<VacancySaga>("vacancysaga", SagaGuid);

            switch (saga.State)
            {
                case VacancyStatus.Published:
                    if (saga.PublishEndDate <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaSwitchedInCommittee());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        mediator.Send(new SwitchVacancyInCommitteeCommand
                        {
                            VacancyGuid = SagaGuid
                        });
                    }
                    break;

                case VacancyStatus.InCommittee:
                    //TODO вынести сроки в конфиг
                    if (!saga.FirstInCommitteeNotificationSent && saga.InCommitteeEndDate.AddDays(-1) <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaFirstInCommitteeNotificationSent());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        throw new NotImplementedException();
                        //TODO отправить уведомление, что пора бы опубликовывать протокол комиссии (скоро истекут 15 или 30 дней)
                    }
                    //TODO вынести сроки в конфиг
                    if (saga.InCommitteeEndDate.AddDays(2) <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaSecondInCommitteeNotificationSent());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        throw new NotImplementedException();
                        //TODO отправить уведомление, что скоро истекут сроки (3 дня) на публикацию протокола (а 15 или 30 дней уже прошли)

                        //отправили два увеодмления, теперь ничего не делаем и просто ждём, когда организация выберет победителя и претендта(необязательно) и приложит
                        //документ с рейтингом заявок
                        scheduler.RemoveScheduledJob(SagaGuid);
                    }

                    break;

                case VacancyStatus.OfferResponseAwaitingFromWinner:
                    //высылаем победителю уведомление, что пора бы принять решение по предложению контракта
                    if (!saga.OfferResponseNotificationSentToWinner && saga.OfferResponseAwaitingFromWinnerEndDate.AddDays(-1) <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaOfferResponseNotificationSentToWinner());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        throw new NotImplementedException();
                        //TODO отправить уведомление
                    }
                    if (saga.OfferResponseAwaitingFromWinnerEndDate <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaOfferRejectedByWinner());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);


                        mediator.Send(new SetWinnerRejectOfferCommand
                        {
                            VacancyGuid = SagaGuid
                        });

                        //перевели вакансию в статус "предложение контракта отклонено победителем" и ждём решения от организации (отменять вакансию или отправить оффер претенднету)
                        scheduler.RemoveScheduledJob(SagaGuid);
                    }

                    break;

                case VacancyStatus.OfferResponseAwaitingFromPretender:
                    //высылаем претенденту уведомление, что пора бы принять решение по предложению контракта
                    if (!saga.OfferResponseNotificationSentToPretender && saga.OfferResponseAwaitingFromPretenderEndDate.AddDays(-1) <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaOfferResponseNotificationSentToPretender());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        throw new NotImplementedException();
                        //TODO отправить уведомление
                    }
                    if (saga.OfferResponseAwaitingFromPretenderEndDate <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaOfferRejectedByPretender());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        mediator.Send(new SetPretenderRejectOfferCommand
                        {
                            VacancyGuid = SagaGuid
                        });

                        saga.Transition(new VacancySagaCancelled());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        mediator.Send(new CancelVacancyCommand
                        {
                            VacancyGuid = SagaGuid,
                            Reason = "Pretender didn't click the button"
                        });

                        //перевели вакансию в статус "предложение контракта отклонено претендентом" и ждём решения от организации
                        scheduler.RemoveScheduledJob(SagaGuid);
                    }
                    break;

                default:
                    //Если приняли офер или отказались от него, то (если отказался победитель) - ждём решения организации, отправлять ли оффер претенденту
                    scheduler.RemoveScheduledJob(SagaGuid);

                    break;
            }
        }
    }
}
