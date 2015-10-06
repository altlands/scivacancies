using Nest;
using Newtonsoft.Json;
using Microsoft.Framework.ConfigurationModel;
using Npgsql;
using NPoco;
using Quartz;
using SciVacancies.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Autofac;
using SciVacancies.ReadModel.Core;
using SciVacancies.Services.Email;
using SciVacancies.Services.Elastic;
using SciVacancies.SmtpNotifications;
using Vacancy = SciVacancies.ReadModel.ElasticSearchModel.Model.Vacancy;

namespace SciVacancies.SearchSubscriptionsService
{
    public interface ISearchSubscriptionScanner
    {
        void PoolHandleSubscriptions();
        void Initialize(EventWaitHandle doneEvent, IEnumerable<SearchSubscription> subscriptionQueue);
        void Initialize(IEnumerable<SearchSubscription> subscriptionQueue);
    }

    /// <summary>
    /// выполнять обработку поискового запроса (запросов)
    /// </summary>
    public class SearchSubscriptionScanner : ISearchSubscriptionScanner
    {
        private IEnumerable<SearchSubscription> _subscriptionQueue;
        private EventWaitHandle _doneEvent;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IConfiguration _configuration;
        readonly IElasticService elasticService;

        public SearchSubscriptionScanner(ILifetimeScope lifetimeScope, IConfiguration configuration, IElasticService elasticService)
        {
            _lifetimeScope = lifetimeScope;
            _configuration = configuration;
            this.elasticService = elasticService;
        }

        public void Initialize(EventWaitHandle doneEvent, IEnumerable<SearchSubscription> subscriptionQueue)
        {
            _doneEvent = doneEvent;
            _subscriptionQueue = subscriptionQueue;
        }
        public void Initialize(IEnumerable<SearchSubscription> subscriptionQueue)
        {
            _subscriptionQueue = subscriptionQueue;
            Console.WriteLine($"                 SearchSubscriptionScanner: Initialize: в сканер передали {subscriptionQueue?.Count()} подписок");
        }

        public void PoolHandleSubscriptions()
        {

            if (_subscriptionQueue == null || !_subscriptionQueue.Any())
            {
                Console.WriteLine("                 список подписок пустой");
                return;
            }
            Console.WriteLine($"                 список подписок содержит - {_subscriptionQueue.Count()} - записей");

            //todo добавить свойство int emailsToSentPerMinute
            //todo: написать sql-процедуру, которая будет блокировать обрабатываемые подписки
            foreach (var searchSubscription in _subscriptionQueue)
            {
                var searchQuery = new SearchQuery
                {
                    Query = searchSubscription.query,
                    PublishDateFrom = searchSubscription.currentcheck_date,
                    FoivIds = JsonConvert.DeserializeObject<IEnumerable<int>>(searchSubscription.foiv_ids),
                    PositionTypeIds = JsonConvert.DeserializeObject<IEnumerable<int>>(searchSubscription.positiontype_ids),
                    RegionIds = JsonConvert.DeserializeObject<IEnumerable<int>>(searchSubscription.region_ids),
                    ResearchDirectionIds = JsonConvert.DeserializeObject<IEnumerable<int>>(searchSubscription.researchdirection_ids),
                    SalaryFrom = searchSubscription.salary_from,
                    SalaryTo = searchSubscription.salary_to,
                    VacancyStatuses = JsonConvert.DeserializeObject<IEnumerable<VacancyStatus>>(searchSubscription.vacancy_statuses)
                };
                Console.WriteLine($"                    searchQuery created based on searchSubscription.Guid='{searchSubscription.guid}' ");

                var searchResults = elasticService.VacancySearch(searchQuery);

                var vacanciesList = searchResults.Documents.ToList();
                Console.WriteLine($"                    по подписке найдено {vacanciesList.Count} записей");

                if (vacanciesList.Count > 0)
                {
                    //    using (var transaction = dataBase.GetTransaction())
                    //    {
                    //        dataBase.Execute(new Sql("UPDATE res_searchsubscriptions SET currenttotal_count = @0, currentcheck_date = @1, lasttotal_count = @2, lastcheck_date = @3 WHERE guid = @4", vacanciesList.Count, DateTime.UtcNow, searchSubscription.currenttotal_count, searchSubscription.currentcheck_date, searchSubscription.guid));
                    //        transaction.Complete();
                    //    }

                    //    var researcher = dataBase.SingleOrDefaultById<SciVacancies.ReadModel.Core.Researcher>(searchSubscription.researcher_guid);
                    //    winnerSetSmtpNotificator.Send(searchSubscription, researcher, vacanciesList);

                    #region SmtpNotifications

                    using (var scope1 = _lifetimeScope.BeginLifetimeScope("scope1"))
                    {
                        var db = scope1.Resolve<IDatabase>();
                        var smtpNotificatorService = scope1.Resolve<ISmtpNotificatorService>();

                        var researcher = db.SingleOrDefaultById<Researcher>(searchSubscription.researcher_guid);
                        var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";

                        Console.WriteLine($"Для {researcherFullName} отправлено письмо на почту {researcher.email}");

                        var body = $@"
<div style=''>
    Уважаемый(-ая), {researcherFullName}, по одной из ваших
    <a target='_blank' href='http://{smtpNotificatorService.Domain}/researcher/subscriptions/'>подписок</a>
     ('{searchSubscription.title}') подобраны следующие вакансии: <br/>
    {vacanciesList.Aggregate(string.Empty, (current, vacancy) => current + $"<a target='_blank' href='http://{smtpNotificatorService.Domain}/vacancies/card/{vacancy.Id}'>{vacancy.FullName}</a> <br/>")}
</div>

<br/>
<br/>
<hr/>

<div style='color: lightgray; font-size: smaller;'>
    Это письмо создано автоматически с 
    <a target='_blank' href='http://{smtpNotificatorService.Domain}'>Портала вакансий</a>.
    Чтобы не получать такие уведомления отключите их или смените email в 
    <a target='_blank' href='http://{smtpNotificatorService.Domain}/researchers/account/'>личном кабинете</a>.
</div>
";
                        smtpNotificatorService.Send(new SciVacMailMessage(researcher.email, "Уведомление с портала вакансий", body));
                    }
                    #endregion
                }
            }

            _doneEvent?.Set();
        }
    }
}