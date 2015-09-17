using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Autofac;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Quartz.Core;

namespace SciVacancies.SearchSubscriptionsService
{
    public class SearchSubscriptionService : ServiceBase
    {
        readonly int MinuteInterval;
        private readonly ILifetimeScope _lifetimeScope;

        public SearchSubscriptionService(ILifetimeScope lifetimeScope)
        {
            MinuteInterval = 1;
            _lifetimeScope = lifetimeScope;
        }

        protected override void OnStart(string[] args)
        {

            //todo: this work should be done in the Job
            var manager = _lifetimeScope.Resolve<ISearchSubscriptionManager>();
            manager.Combine();

            //base.OnStart(args);

            ////logging

            ////quartz
            //try
            //{
            //    ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            //    var scheduler = schedulerFactory.GetScheduler();

            //    var jobKey = new JobKey("SciVacancies.SearchSubscriptionJob", "SciVacancies.SearchSubscriptionService");
            //    var triggerKey = new TriggerKey("SciVacancies.SearchSubscriptionJobTrigger", "SciVacancies.SearchSubscriptionService");

            //    if (scheduler.CheckExists(jobKey) && scheduler.CheckExists(triggerKey))
            //    {
            //        scheduler.DeleteJob(jobKey);
            //    }

            //    var job = JobBuilder.Create<SearchSubscriptionJob>()
            //                                .WithIdentity(jobKey)
            //                                .Build();

            //    var trigger = TriggerBuilder.Create()
            //                                    .WithIdentity(triggerKey)
            //                                    .WithSimpleSchedule(s => s
            //                                        .WithIntervalInMinutes(MinuteInterval)
            //                                        .RepeatForever())
            //                                    .Build();

            //    scheduler.ScheduleJob(job, trigger);

            //    scheduler.Start();
            //}
            //catch (Exception e)
            //{

            //}

        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}
