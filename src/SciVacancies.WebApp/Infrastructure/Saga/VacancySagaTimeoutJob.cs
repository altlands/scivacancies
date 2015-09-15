using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Aggregates;
using SciVacancies.WebApp.Infrastructure.Saga;

using System;

using Quartz;
using SciVacancies.Services.Quartz;

namespace SciVacancies.WebApp.Infrastructure
{
    public class VacancySagaTimeoutJob : IJob
    {
        public Guid SagaGuid;

        readonly ISagaRepository sagaRepository;
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
                    if (!saga.FirstInCommitteNotificationSent && saga.OfferResponseAwaitingEndDate.AddDays(-1) <= DateTime.UtcNow)
                    {
                        //TODO отправить уведомление, что пора бы опубликовывать протокол комиссии (скоро истекут 15 или 30 дней)
                    }
                    //TODO вынести сроки в конфиг
                    if (saga.OfferResponseAwaitingEndDate.AddDays(2) <= DateTime.UtcNow)
                    {
                        //TODO отправить уведомление, что скоро истекут сроки (3 дня) на публикацию протокола (а 15 или 30 дней уже прошли)

                        //отправили два увеодмления, теперь ничего не делаем и просто ждём, когда организация выберет победителя и претендта(необязательно) и приложит
                        //документ с рейтингом заявок
                        scheduler.RemoveScheduledJob(SagaGuid);
                    }

                    break;

                case VacancyStatus.OfferResponseAwaiting:
                    if (!saga.OfferResponseNotificationSent && saga.OfferResponseAwaitingEndDate.AddDays(-1) <= DateTime.UtcNow)
                    {
                        //TODO ожидаем ответа победителя 30 дней, за 1 день до конца срока отправляем уведомления и победителю и организации

                    }
                    if (saga.OfferResponseAwaitingEndDate <= DateTime.UtcNow)
                    {
                        saga.Transition(new VacancySagaOfferRejected());
                        sagaRepository.Save("vacancysaga", saga, Guid.NewGuid(), null);

                        Vacancy vacancy = repository.GetById<Vacancy>(saga.VacancyGuid);
                        vacancy.WinnerRejectOffer();
                        repository.Save(vacancy, Guid.NewGuid(), null);
                    }
                    //TODO если победитель согласился, то у организации есть кнопка закрыть или отменить
                    //TODO ожидаем ответа претендента 30 дней, за 1 день до конца срока отправляем уведомления и претенденту и организации
                    //TODO если претендент не успел нажать или отказался, то автоматом переводим на отменено
                    //TODO если претендент согласился, то у организации есть кнопка закрыть или отменить
                    break;

                default:
                    //Если приняли офер или отказались от него, то (если отказался победитель) - ждём решения организации, отправлять ли оффер претенденту
                    scheduler.RemoveScheduledJob(SagaGuid); break;

                    break;
            }
        }
    }
}
