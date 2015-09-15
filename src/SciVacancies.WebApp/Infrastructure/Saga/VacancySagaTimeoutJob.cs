using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Aggregates;
using SciVacancies.WebApp.Infrastructure.Saga;

using System;

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

        /// <summary>
        /// Репозиторий с агрегатами
        /// </summary>
        readonly CommonDomain.Persistence.IRepository repository;

        readonly ISchedulerService scheduler;

        public VacancySagaTimeoutJob()
        {
            //оставляем пустым
        }

        public VacancySagaTimeoutJob(ISagaRepository sagaRepository, CommonDomain.Persistence.IRepository repository, ISchedulerService scheduler)
        {
            this.sagaRepository = sagaRepository;
            this.repository = repository;
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

                        Vacancy vacancy = repository.GetById<Vacancy>(saga.VacancyGuid);
                        vacancy.VacancyToCommittee();
                        repository.Save(vacancy, Guid.NewGuid(), null);
                    }
                    break;

                case VacancyStatus.InCommittee:
                    //TODO вынести сроки в конфиг
                    if (!saga.FirstInCommitteeNotificationSent && saga.InCommitteeEndDate.AddDays(-1) <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaFirstInCommitteeNotificationSent());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        //TODO отправить уведомление, что пора бы опубликовывать протокол комиссии (скоро истекут 15 или 30 дней)
                    }
                    //TODO вынести сроки в конфиг
                    if (saga.InCommitteeEndDate.AddDays(2) <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaSecondInCommitteeNotificationSent());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

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

                        //TODO отправить уведомление
                    }
                    if (saga.OfferResponseAwaitingFromWinnerEndDate <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaOfferRejectedByWinner());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        Vacancy vacancy = repository.GetById<Vacancy>(saga.VacancyGuid);
                        vacancy.WinnerRejectOffer();
                        repository.Save(vacancy, Guid.NewGuid(), null);

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

                        //TODO отправить уведомление
                    }
                    if (saga.OfferResponseAwaitingFromPretenderEndDate <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaOfferRejectedByPretender());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        Vacancy vacancy = repository.GetById<Vacancy>(saga.VacancyGuid);
                        vacancy.PretenderRejectOffer();
                        repository.Save(vacancy, Guid.NewGuid(), null);

                        saga.Transition(new VacancySagaCancelled());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        vacancy.Cancel("Pretender didn't click the button");
                        repository.Save(vacancy, Guid.NewGuid(), null);

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
