using Newtonsoft.Json;
using NPoco;
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
using Microsoft.Framework.Configuration;

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
        readonly IElasticService _elasticService;

        readonly string From;
        readonly string Domain;
        readonly string PortalLink;


        public SearchSubscriptionScanner(ILifetimeScope lifetimeScope, IElasticService elasticService, IConfiguration configuration)
        {
            _lifetimeScope = lifetimeScope;
            _elasticService = elasticService;

            this.From = configuration["EmailSettings:Login"];
            this.Domain = configuration["EmailSettings:Domain"];
            this.PortalLink = configuration["EmailSettings:PortalLink"];

            if (string.IsNullOrEmpty(this.From)) throw new ArgumentNullException("From is null");
            if (string.IsNullOrEmpty(this.Domain)) throw new ArgumentNullException("Domain is null");
            if (string.IsNullOrEmpty(this.PortalLink)) throw new ArgumentNullException("PortalLink is null");
        }

        public void Initialize(EventWaitHandle doneEvent, IEnumerable<SearchSubscription> subscriptionQueue)
        {
            _doneEvent = doneEvent;
            _subscriptionQueue = subscriptionQueue;
        }
        public void Initialize(IEnumerable<SearchSubscription> subscriptionQueue)
        {
            _subscriptionQueue = subscriptionQueue;
        }

        public void PoolHandleSubscriptions()
        {

            if (_subscriptionQueue == null || !_subscriptionQueue.Any())
                return;

            Console.WriteLine($"                 SearchSubscriptionScanner: список подписок содержит -{_subscriptionQueue.Count()}- записей");

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
                Console.WriteLine($"                 SearchSubscriptionScanner: searchQuery создан из подписки '{searchSubscription.guid}' ");

                var searchResults = _elasticService.VacancySearch(searchQuery);

                var vacanciesList = searchResults.Documents.ToList();
                Console.WriteLine($"                 SearchSubscriptionScanner: по подписке найдено {vacanciesList.Count} записей");

                if (vacanciesList.Count > 0)
                {
                    #region SmtpNotifications
                    
                    using (var scopePerEmail = _lifetimeScope.BeginLifetimeScope("scopePerEmail"+ Guid.NewGuid()))
                    {
                        var db = scopePerEmail.Resolve<IDatabase>();
                        var EmailService = scopePerEmail.Resolve<IEmailService>();

                        var researcher = db.SingleOrDefaultById<Researcher>(searchSubscription.researcher_guid);
                        var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
                        var body = $@"
<div style=''>
    Уважаемый(-ая), {researcherFullName}, по одной из ваших
    <a target='_blank' href='http://{Domain}/researcher/subscriptions/'>подписок</a>
     ('{searchSubscription.title}') подобраны следующие вакансии: <br/>
    {vacanciesList.Aggregate(string.Empty, (current, vacancy) => current + $"<a target='_blank' href='http://{Domain}/vacancies/card/{vacancy.Id}'>{vacancy.FullName}</a> <br/>")}
</div>

<br/>
<br/>
<hr/>

<div style='color: lightgray; font-size: smaller;'>
    Это письмо создано автоматически с 
    <a target='_blank' href='http://{Domain}'>Портала вакансий</a>.
    Чтобы не получать такие уведомления отключите их или смените email в 
    <a target='_blank' href='http://{Domain}/researchers/account/'>личном кабинете</a>.
</div>
";
                        EmailService.Send(new SciVacMailMessage(From,researcher.email, "Уведомление с портала вакансий", body));

                        Console.WriteLine($"                 SearchSubscriptionScanner: письмо {researcher.email} для {researcherFullName}");
                    }
                    #endregion
                }
            }

            _doneEvent?.Set();
        }
    }
}