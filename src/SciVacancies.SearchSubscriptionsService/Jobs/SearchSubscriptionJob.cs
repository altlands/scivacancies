using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.ElasticSearchModel.Model;
using SciVacancies.SmtpNotifications;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using NPoco;
using Npgsql;
using Nest;
using Newtonsoft.Json;

namespace SciVacancies.SearchSubscriptionsService
{
    public class SearchSubscriptionJob : IJob
    {
        static readonly string ServiceId="#1";
        static readonly int Seed=1;

        readonly IEmailService _emailService;
        //readonly IElasticService _elasticService;
        readonly IDatabase _db;

        //TODO
        public SearchSubscriptionJob(IEmailService emailService, IDatabase db)
        {
            _emailService = emailService;
            //_elasticService = elasticService;
            _db = db;
        }

        public void Execute(IJobExecutionContext context)
        {
            //    var dataBase = new Database("Server = localhost; Database = scivacancies; User Id = postgres; Password = postgres", NpgsqlFactory.Instance);
            //    var elasticClient = new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200/"), defaultIndex: "scivacancies"));

            //    //TODO - загружать по частям
            //    Queue<SciVacancies.ReadModel.Core.SearchSubscription> subscriptionQueue = new Queue<ReadModel.Core.SearchSubscription>(dataBase.Fetch<SciVacancies.ReadModel.Core.SearchSubscription>(new Sql($"SELECT * FROM res_searchsubscriptions ss WHERE ss.status = @0", SearchSubscriptionStatus.Active)));

            //    //int emailsToSentPerMinute
            //    while (subscriptionQueue.Count > 0)
            //    {
            //        var searchSubscription = subscriptionQueue.Dequeue();
            //        //TODO

            //        var searchQuery = new SearchQuery
            //        {
            //            Query = searchSubscription.query,
            //            PublishDateFrom = searchSubscription.currentcheck_date,
            //            FoivIds = JsonConvert.DeserializeObject<IEnumerable<int>>(searchSubscription.foiv_ids),
            //            PositionTypeIds = JsonConvert.DeserializeObject<IEnumerable<int>>(searchSubscription.positiontype_ids),
            //            RegionIds = JsonConvert.DeserializeObject<IEnumerable<int>>(searchSubscription.region_ids),
            //            ResearchDirectionIds = JsonConvert.DeserializeObject<IEnumerable<int>>(searchSubscription.researchdirection_ids),
            //            SalaryFrom = searchSubscription.salary_from,
            //            SalaryTo = searchSubscription.salary_to,
            //            VacancyStatuses = JsonConvert.DeserializeObject<IEnumerable<VacancyStatus>>(searchSubscription.vacancy_statuses)
            //        };

            //        Func<SearchQuery, SearchDescriptor<Vacancy>> searchSelector = VacancySearchDescriptor;

            //        var searchResults = elasticClient.Search<Vacancy>(searchSelector(searchQuery));
            //        var vacanciesList = searchResults.Documents.ToList();

            //        //if (vacanciesList.Count > 0)
            //        //{
            //        //    using (var transaction = dataBase.GetTransaction())
            //        //    {
            //        //        dataBase.Execute(new Sql("UPDATE res_searchsubscriptions SET currenttotal_count = @0, currentcheck_date = @1, lasttotal_count = @2, lastcheck_date = @3 WHERE guid = @4", vacanciesList.Count, DateTime.UtcNow, searchSubscription.currenttotal_count, searchSubscription.currentcheck_date, searchSubscription.guid));
            //        //        transaction.Complete();
            //        //    }

            //        //    var researcher = dataBase.SingleOrDefaultById<SciVacancies.ReadModel.Core.Researcher>(searchSubscription.researcher_guid);
            //        //    winnerSetSmtpNotificator.Send(searchSubscription, researcher, vacanciesList);

            //        //}
            //    }
            //}

            //public class SearchQuery
            //{
            //    public string Query { get; set; }

            //    public long? PageSize { get; set; }
            //    public long? CurrentPage { get; set; }

            //    public string OrderFieldByDirection { get; set; }

            //    public DateTime? PublishDateFrom { get; set; }

            //    public IEnumerable<int> FoivIds { get; set; }
            //    public IEnumerable<int> PositionTypeIds { get; set; }
            //    public IEnumerable<int> RegionIds { get; set; }
            //    public IEnumerable<int> ResearchDirectionIds { get; set; }

            //    public int? SalaryFrom { get; set; }
            //    public int? SalaryTo { get; set; }
            //    public IEnumerable<VacancyStatus> VacancyStatuses { get; set; }
            //}

            //#region QueryContainers

            //public SearchDescriptor<Vacancy> VacancySearchDescriptor(SearchQuery sq)
            //{
            //    SearchDescriptor<Vacancy> sdescriptor = new SearchDescriptor<Vacancy>();

            //    if (sq.PageSize.HasValue && sq.CurrentPage.HasValue &&
            //        sq.PageSize.Value != 0 && sq.CurrentPage.Value != 0)
            //    {
            //        sdescriptor.Skip((int)((sq.CurrentPage - 1) * sq.PageSize));
            //        sdescriptor.Take((int)sq.PageSize);
            //    }

            //    sdescriptor.Sort(sort => sort.OnField(of => of.PublishDate).Descending());

            //    Func<SearchQuery, QueryContainer> querySelector = VacancyQueryContainer;

            //    sdescriptor.Query(querySelector(sq));

            //    return sdescriptor;
            //}
            //public QueryContainer VacancyQueryContainer(SearchQuery sq)
            //{

            //    QueryContainer query = Query<Vacancy>.Filtered(fltd => fltd
            //                                                .Query(q => q
            //                                                    .FuzzyLikeThis(flt => flt
            //                                                        .LikeText(sq.Query)
            //                                                    )
            //                                                )
            //                                                .Filter(f => f
            //                                                    .Terms<int>(ft => ft.OrganizationFoivId, sq.FoivIds)
            //                                                    && f.Terms<int>(ft => ft.PositionTypeId, sq.PositionTypeIds)
            //                                                    && f.Terms<int>(ft => ft.RegionId, sq.RegionIds)
            //                                                    && f.Terms<int>(ft => ft.ResearchDirectionId, sq.ResearchDirectionIds)
            //                                                    && f.Range(fr => fr
            //                                                            .GreaterOrEquals((sq.SalaryFrom.HasValue && sq.SalaryFrom > 0) ? (long?)sq.SalaryFrom : null)
            //                                                            .OnField(of => of.SalaryFrom)
            //                                                    )
            //                                                    && f.Range(fr => fr
            //                                                            .LowerOrEquals((sq.SalaryTo.HasValue && sq.SalaryTo < 0) ? (long?)sq.SalaryTo : null)
            //                                                            .OnField(of => of.SalaryTo)
            //                                                    )
            //                                                    && f.Terms<VacancyStatus>(ft => ft.Status, sq.VacancyStatuses)
            //                                                    && f.Range(fr => fr
            //                                                            .GreaterOrEquals(sq.PublishDateFrom)
            //                                                            .OnField(of => of.PublishDate)
            //                                                    )
            //                                                    && f.Bool(b => b
            //                                                            .MustNot(mn => mn
            //                                                                .Terms<VacancyStatus>(mnt => mnt.Status, new List<VacancyStatus> { VacancyStatus.InProcess })
            //                                                            )
            //                                                    )
            //                                                )
            //                                        );

            //    return query;
            //}

            //#endregion
        }
    }
}
