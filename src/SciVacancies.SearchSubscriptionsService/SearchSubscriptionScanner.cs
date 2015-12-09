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
using Microsoft.Framework.Logging;

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
        readonly ISearchService _elasticService;

        private readonly IDatabase _db;

        private readonly ILogger _logger;

        readonly string _from;
        readonly string _domain;
        readonly string _portalLink;


        public SearchSubscriptionScanner(ILifetimeScope lifetimeScope, ISearchService elasticService, IDatabase db, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<SearchSubscriptionScanner>();
            _logger.LogInformation("SearchSubscriptionScanner: ctor initializing");
            _lifetimeScope = lifetimeScope;
            _elasticService = elasticService;
            this._db = db;

            if (configuration == null)
                _logger.LogInformation("SearchSubscriptionScanner: configuration is null ");

            _logger.LogInformation("SearchSubscriptionScanner: getting EmailSettings:Login");
            this._from = configuration["EmailSettings:Login"];
            _logger.LogInformation("SearchSubscriptionScanner: getting EmailSettings:Domain");
            this._domain = configuration["EmailSettings:Domain"];
            _logger.LogInformation("SearchSubscriptionScanner: getting EmailSettings:PortalLink");
            this._portalLink = configuration["EmailSettings:PortalLink"];

            if (string.IsNullOrEmpty(this._from)) throw new ArgumentNullException("From is null");
            _logger.LogInformation("SearchSubscriptionScanner: got From");
            if (string.IsNullOrEmpty(this._domain)) throw new ArgumentNullException("Domain is null");
            _logger.LogInformation("SearchSubscriptionScanner: got Domain");
            if (string.IsNullOrEmpty(this._portalLink)) throw new ArgumentNullException("PortalLink is null");
            _logger.LogInformation("SearchSubscriptionScanner: got PortalLink");
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

            _logger.LogInformation($"SearchSubscriptionScanner: список подписок содержит -{_subscriptionQueue.Count()}- записей");

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
                _logger.LogInformation($"SearchSubscriptionScanner: searchQuery создан из подписки '{searchSubscription.guid}' ");

                var searchResults = _elasticService.VacancySearch(searchQuery);

                var total = searchResults.Total;

                searchSubscription.lastcheck_date = searchSubscription.currentcheck_date;
                searchSubscription.lasttotal_count = searchSubscription.currenttotal_count;

                searchSubscription.currentcheck_date = DateTime.UtcNow;
                searchSubscription.currenttotal_count = total;

                var vacanciesList = searchResults.Documents.ToList();
                _logger.LogInformation($"SearchSubscriptionScanner: по подписке найдено {vacanciesList.Count} записей");

                if (vacanciesList.Count > 0)
                {
                    #region SmtpNotifications

                    using (var scopePerEmail = _lifetimeScope.BeginLifetimeScope("scopePerEmail" + Guid.NewGuid()))
                    {
                        var db = scopePerEmail.Resolve<IDatabase>();
                        var emailService = scopePerEmail.Resolve<IEmailService>();

                        var researcher = db.SingleOrDefaultById<Researcher>(searchSubscription.researcher_guid);
                        var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
                        var body = $@"
                                    <div style=''>
                                        Уважаемый(-ая), {researcherFullName}, по одной из ваших
                                        <a target='_blank' href='http://{_domain}/researcher/subscriptions/'>подписок</a>
                                         ('{searchSubscription.title}') подобраны следующие вакансии: <br/>
                                        {vacanciesList.Aggregate(string.Empty, (current, vacancy) => current + $"<a target='_blank' href='http://{_domain}/vacancies/card/{vacancy.Id}'>{vacancy.Name}</a> <br/>")}
                                    </div>

                                    <br/>
                                    <br/>
                                    <hr/>

                                    <div style='color: lightgray; font-size: smaller;'>
                                        Это письмо создано автоматически с 
                                        <a target='_blank' href='http://{_domain}'>Портала вакансий</a>.
                                        Чтобы не получать такие уведомления отключите их или смените email в 
                                        <a target='_blank' href='http://{_domain}/researchers/account/'>личном кабинете</a>.
                                    </div>
                                    ";
                        using (
                            var message = new SciVacMailMessage(_from, researcher.email,
                                "Уведомление с портала вакансий по Вашей поисковой подписке", body))
                        {
                            emailService.Send(message);
                        }

                        _logger.LogInformation($"SearchSubscriptionScanner: письмо {researcher.email} для {researcherFullName} отправлено");
                    }
                    #endregion
                }
            }

            using (var transaction = _db.GetTransaction())
            {
                foreach (SearchSubscription searchSubscription in _subscriptionQueue)
                {
                    _db.Update(searchSubscription);
                }

                transaction.Complete();
            }

            _doneEvent?.Set();
        }
    }
}