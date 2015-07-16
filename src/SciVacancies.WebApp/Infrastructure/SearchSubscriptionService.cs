using SciVacancies.Domain.Enums;
//using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SciVacancies.WebApp.Queries;
using MediatR;
using NPoco;
using Nest;
using Quartz;
using Quartz.Impl;
using Newtonsoft.Json;
using Npgsql;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Engine.SmtpNotificators;
using Vacancy = SciVacancies.ReadModel.ElasticSearchModel.Model.Vacancy;

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
                Database db = new Database("Server = localhost; Database = scivacancies; User Id = postgres; Password = postgres", NpgsqlFactory.Instance);
                ElasticClient elasticClient = new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200/"), defaultIndex: "scivacancies"));
                IEnumerable<SciVacancies.ReadModel.Core.SearchSubscription> searchsubscriptions = db.Fetch<SciVacancies.ReadModel.Core.SearchSubscription>(new Sql($"SELECT * FROM res_searchsubscriptions ss WHERE ss.status = @0", SearchSubscriptionStatus.Active));

                var winnerSetSmtpNotificator = new WinnerSetSmtpNotificator();
                
                foreach (SciVacancies.ReadModel.Core.SearchSubscription searchSubscription in searchsubscriptions)
                {
                    SearchQuery searchQuery = new SearchQuery
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
                    List<Vacancy> vacancies = searchResults.Documents.ToList();

                    if (vacancies.Count > 0)
                    {
                        using (var transaction = db.GetTransaction())
                        {
                            db.Execute(new Sql("UPDATE res_searchsubscriptions SET currenttotal_count = @0, currentcheck_date = @1, lasttotal_count = @2, lastcheck_date = @3 WHERE guid = @4", vacancies.Count, DateTime.UtcNow, searchSubscription.currenttotal_count, searchSubscription.currentcheck_date, searchSubscription.guid));
                            transaction.Complete();
                        }

                        //TODO - отправка письма счастья
                        var researcher = db.SingleOrDefaultById<Researcher>(searchSubscription.researcher_guid);
                        winnerSetSmtpNotificator.Send(searchSubscription,researcher, vacancies);

                    }
                }
            }
            #region QueryContainers

            public SearchDescriptor<Vacancy> VacancySearchDescriptor(SearchQuery sq)
            {
                SearchDescriptor<Vacancy> sdescriptor = new SearchDescriptor<Vacancy>();

                sdescriptor.Index("scivacancies");
                if (sq.PageSize.HasValue && sq.CurrentPage.HasValue)
                //sq.PageSize.Value != 0 && sq.CurrentPage.Value != 0)
                {
                    sdescriptor.Skip((int)((sq.CurrentPage - 1) * sq.PageSize));
                    sdescriptor.Take((int)sq.PageSize);
                }
                switch (sq.OrderBy)
                {
                    case ConstTerms.OrderByDateDescending:
                        sdescriptor.Sort(sort => sort.OnField(of => of.PublishDate).Descending());
                        break;
                    case ConstTerms.OrderByDateAscending:
                        sdescriptor.Sort(sort => sort.OnField(of => of.PublishDate).Ascending());
                        break;
                }
                Func<SearchQuery, QueryContainer> querySelector = VacancyQueryContainer;

                sdescriptor.Query(querySelector(sq));

                return sdescriptor;
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
