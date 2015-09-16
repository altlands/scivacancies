using Nest;
using Newtonsoft.Json;
using Npgsql;
using NPoco;
using Quartz;
using SciVacancies.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SciVacancies.ReadModel.Core;
using SciVacancies.Services.Email;
using SciVacancies.SmtpNotifications;
using SciVacancies.SmtpNotifications.SmtpNotificators;
using Vacancy = SciVacancies.ReadModel.ElasticSearchModel.Model.Vacancy;

namespace SciVacancies.SearchSubscriptionsService
{
    /// <summary>
    /// выполнять обработку поискового запроса (запросов)
    /// </summary>
    public class SearchSubscriptionScanner : ISearchSubscriptionScanner
    {
        private IEnumerable<SearchSubscription> _subscriptionQueue;
        private readonly ISmtpNotificatorService _smtpNotificatorService;
        private ManualResetEvent _doneEvent;

        public SearchSubscriptionScanner(ISmtpNotificatorService smtpNotificatorService)
        {
            _smtpNotificatorService = smtpNotificatorService;
        }

        public void Initialize(ManualResetEvent doneEvent, IEnumerable<SearchSubscription> subscriptionQueue)
        {
            _doneEvent = doneEvent;
            _subscriptionQueue = subscriptionQueue;
        }
        /// <summary>
        /// обработать несколько запросов
        /// </summary>
        public void PoolHandleSubscriptions(object threadContext)
        {
            if (_subscriptionQueue == null)
                return;

            var elasticClient = new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200/"), defaultIndex: "scivacancies"));

            //todo int emailsToSentPerMinute
            foreach (var searchSubscription in _subscriptionQueue)
            {
                //TODO

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

                Func<SearchQuery, SearchDescriptor<Vacancy>> searchSelector = VacancySearchDescriptor;

                var searchResults = elasticClient.Search<Vacancy>(searchSelector(searchQuery));
                var vacanciesList = searchResults.Documents.ToList();

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
                    var researcher = new Researcher();
                    var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
                    var body = $@"
<div style=''>
    Уважаемый(-ая), {researcherFullName}, по одной из ваших
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}/researcher/subscriptions/'>подписок</a>
     ('{searchSubscription.title}') подобраны следующие вакансии: <br/>
    {vacanciesList.Aggregate(string.Empty, (current, vacancy) => current + $"<a target='_blank' href='http://{_smtpNotificatorService.Domain}/vacancies/card/{vacancy.Id}'>{vacancy.FullName}</a> <br/>")}
</div>

<br/>
<br/>
<hr/>

<div style='color: lightgray; font-size: smaller;'>
    Это письмо создано автоматически с 
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}'>Портала вакансий</a>.
    Чтобы не получать такие уведомления отключите их или смените email в 
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}/researchers/account/'>личном кабинете</a>.
</div>
";
                    _smtpNotificatorService.Send(new SciVacMailMessage(researcher.email, "Уведомление с портала вакансий", body));
                    #endregion
                }
            }


            _doneEvent?.Set();

        }


        #region QueryContainers

        public SearchDescriptor<Vacancy> VacancySearchDescriptor(SearchQuery sq)
        {
            var sdescriptor = new SearchDescriptor<Vacancy>();

            if (sq.PageSize.HasValue && sq.CurrentPage.HasValue &&
                sq.PageSize.Value != 0 && sq.CurrentPage.Value != 0)
            {
                sdescriptor.Skip((int)((sq.CurrentPage - 1) * sq.PageSize));
                sdescriptor.Take((int)sq.PageSize);
            }

            sdescriptor.Sort(sort => sort.OnField(of => of.PublishDate).Descending());

            Func<SearchQuery, QueryContainer> querySelector = VacancyQueryContainer;

            sdescriptor.Query(querySelector(sq));

            return sdescriptor;
        }
        public QueryContainer VacancyQueryContainer(SearchQuery sq)
        {

            var queryContainer = Query<Vacancy>.Filtered(fltd => fltd
                     .Query(q => q
                         .FuzzyLikeThis(flt => flt
                             .LikeText(sq.Query)
                         )
                     )
                     .Filter(f => f
                         .Terms<int>(ft => ft.OrganizationFoivId, sq.FoivIds)
                         && f.Terms<int>(ft => ft.PositionTypeId, sq.PositionTypeIds)
                         && f.Terms<int>(ft => ft.RegionId, sq.RegionIds)
                         && f.Terms<int>(ft => ft.ResearchDirectionId, sq.ResearchDirectionIds)
                         && f.Range(fr => fr
                                 .GreaterOrEquals((sq.SalaryFrom.HasValue && sq.SalaryFrom > 0) ? (long?)sq.SalaryFrom : null)
                                 .OnField(of => of.SalaryFrom)
                         )
                         && f.Range(fr => fr
                                 .LowerOrEquals((sq.SalaryTo.HasValue && sq.SalaryTo < 0) ? (long?)sq.SalaryTo : null)
                                 .OnField(of => of.SalaryTo)
                         )
                         && f.Terms<VacancyStatus>(ft => ft.Status, sq.VacancyStatuses)
                         && f.Range(fr => fr
                                 .GreaterOrEquals(sq.PublishDateFrom)
                                 .OnField(of => of.PublishDate)
                         )
                         && f.Bool(b => b
                                 .MustNot(mn => mn
                                     .Terms<VacancyStatus>(mnt => mnt.Status, new List<VacancyStatus> { VacancyStatus.InProcess })
                                 )
                         )
                     )
             );

            return queryContainer;
        }
        #endregion



        public class SearchQuery
        {
            public string Query { get; set; }

            public long? PageSize { get; set; }
            public long? CurrentPage { get; set; }

            public string OrderFieldByDirection { get; set; }

            public DateTime? PublishDateFrom { get; set; }

            public IEnumerable<int> FoivIds { get; set; }
            public IEnumerable<int> PositionTypeIds { get; set; }
            public IEnumerable<int> RegionIds { get; set; }
            public IEnumerable<int> ResearchDirectionIds { get; set; }

            public int? SalaryFrom { get; set; }
            public int? SalaryTo { get; set; }
            public IEnumerable<VacancyStatus> VacancyStatuses { get; set; }
        }

    }
}