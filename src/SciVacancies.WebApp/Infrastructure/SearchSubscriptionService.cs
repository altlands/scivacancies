using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;
using Quartz;
using Quartz.Impl;
using Newtonsoft.Json;

namespace SciVacancies.WebApp.Infrastructure
{
    public static class SearchSubscriptionService
    {
        public static void Initialize()
        {
            int HourRun = 13;
            int MinuteRun = 15;
            int HourInterval = 1;

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
                        .WithIntervalInHours(HourInterval)
                        .OnEveryDay()
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(HourRun, MinuteRun)))
                        .Build();

                scheduler.ScheduleJob(job, trigger);
            }
            catch (SchedulerException se)
            {
                throw;
            }
        }
        private class SearchSubscriptionCheck : IJob
        {
            private readonly IMediator _mediator;

            public SearchSubscriptionCheck(IMediator mediator)
            {
                _mediator = mediator;
            }

            public void Execute(IJobExecutionContext context)
            {
                IEnumerable<SearchSubscription> searchsubscriptions = _mediator.Send(new SelectActiveSearchSubscriptionsQuery());

                foreach (SearchSubscription ss in searchsubscriptions)
                {
                    //long currentTotalCount = _mediator.Send(new SearchQuery
                    //{
                    //    Query = ss.query,
                    //    FoivIds = JsonConvert.DeserializeObject<IEnumerable<int>>(ss.foiv_ids),
                    //    PositionTypeIds = JsonConvert.DeserializeObject<IEnumerable<int>>(ss.positiontype_ids),
                    //    RegionIds = JsonConvert.DeserializeObject<IEnumerable<int>>(ss.region_ids),
                    //    ResearchDirectionIds = JsonConvert.DeserializeObject<IEnumerable<int>>(ss.researchdirection_ids),
                    //    SalaryFrom = ss.salary_from,
                    //    SalaryTo = ss.salary_to,
                    //    VacancyStatuses = JsonConvert.DeserializeObject<IEnumerable<VacancyStatus>>(ss.vacancy_statuses)
                    //}).TotalItems;

                    //if (currentTotalCount > ss.lasttotal_count)
                    //{
                    //    _mediator.Send(new UpdateSearchSubscriptionCountersCommand
                    //    {
                    //        SearchSubscriptionGuid = ss.guid,
                    //        CurrentTotalCounter = currentTotalCount,
                    //        LastTotalCounter = ss.currenttotal_count
                    //    });
                    //}
                }
            }
        }
    }
}
