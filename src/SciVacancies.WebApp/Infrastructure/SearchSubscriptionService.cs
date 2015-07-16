using SciVacancies.Domain.Enums;
using SciVacancies.WebApp.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using NPoco;
using Nest;
using Quartz;
using Quartz.Impl;
using Newtonsoft.Json;
using Npgsql;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Engine.SmtpNotificators;
using SciVacancies.ReadModel.ElasticSearchModel.Model;

namespace SciVacancies.WebApp.Infrastructure
{
    public static class SearchSubscriptionService
    {
        public static void Initialize()
        {
            int HourRun = 16;
            int MinuteRun = 00;
            int HourInterval = 10;

            try
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

                scheduler.Start();

                IJobDetail job = JobBuilder.Create<SearchSubscriptionCheck>()
                    .WithIdentity("searchsubscriptioncheck", "group1")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("searchsubscriptiontrigger", "group1")
                    .WithDailyTimeIntervalSchedule(s => s
                        .WithIntervalInMinutes(1)
                        //.WithIntervalInSeconds(HourInterval)
                        .OnEveryDay()
                    //.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(HourRun, MinuteRun))
                    )
                    .Build();

                scheduler.ScheduleJob(job, trigger);
            }
            catch (SchedulerException se)
            {
                throw;
            }
        }
        public class SearchSubscriptionCheck : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                var dataBase = new Database("Server = localhost; Database = scivacancies; User Id = postgres; Password = postgres", NpgsqlFactory.Instance);
                var elasticClient = new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200/"), defaultIndex: "scivacancies"));
                IEnumerable<SciVacancies.ReadModel.Core.SearchSubscription> searchsubscriptions = dataBase.Fetch<SciVacancies.ReadModel.Core.SearchSubscription>(new Sql($"SELECT * FROM res_searchsubscriptions ss WHERE ss.status = @0", SearchSubscriptionStatus.Active));

                var winnerSetSmtpNotificator = new WinnerSetSmtpNotificator();
                
                foreach (SciVacancies.ReadModel.Core.SearchSubscription searchSubscription in searchsubscriptions)
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
                    Func<SearchQuery, SearchDescriptor<Vacancy>> searchSelector = VacancySearchDescriptor;

                    var searchResults = elasticClient.Search<Vacancy>(searchSelector(searchQuery));
                    var vacanciesList = searchResults.Documents.ToList();

                    if (vacanciesList.Count > 0)
                    {
                        using (var transaction = dataBase.GetTransaction())
                        {
                            dataBase.Execute(new Sql("UPDATE res_searchsubscriptions SET currenttotal_count = @0, currentcheck_date = @1, lasttotal_count = @2, lastcheck_date = @3 WHERE guid = @4", vacanciesList.Count, DateTime.UtcNow, searchSubscription.currenttotal_count, searchSubscription.currentcheck_date, searchSubscription.guid));
                            transaction.Complete();
                        }

                        //TODO - отправка письма счастья
                        var researcher = dataBase.SingleOrDefaultById<SciVacancies.ReadModel.Core.Researcher>(searchSubscription.researcher_guid);
                        winnerSetSmtpNotificator.Send(searchSubscription,researcher, vacanciesList);

                    }
                }
            }
            #region QueryContainers

            public SearchDescriptor<Vacancy> VacancySearchDescriptor(SearchQuery sq)
            {
                var searchDescriptor = new SearchDescriptor<Vacancy>();
                searchDescriptor.Index("scivacancies");
                if (sq.PageSize.HasValue && sq.CurrentPage.HasValue)
                //sq.PageSize.Value != 0 && sq.CurrentPage.Value != 0)
                {
                    searchDescriptor.Skip((int)((sq.CurrentPage - 1) * sq.PageSize));
                    searchDescriptor.Take((int)sq.PageSize);
                }
                switch (sq.OrderBy)
                {
                    case ConstTerms.OrderByDateDescending:
                        searchDescriptor.Sort(sort => sort.OnField(of => of.PublishDate).Descending());
                        break;
                    case ConstTerms.OrderByDateAscending:
                        searchDescriptor.Sort(sort => sort.OnField(of => of.PublishDate).Ascending());
                        break;
                }
                Func<SearchQuery, QueryContainer> querySelector = VacancyQueryContainer;

                searchDescriptor.Query(querySelector(sq));

                return searchDescriptor;
            }
            public QueryContainer VacancyQueryContainer(SearchQuery sq)
            {

                QueryContainer query = Query<Vacancy>.Filtered(fltd => fltd
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

                return query;
            }

            #endregion
        }
    }
}
