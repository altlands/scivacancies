using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Quartz.Core;

namespace SciVacancies.SearchSubscriptionsService
{
    public class SearchSubscriptionService : ServiceBase
    {
        readonly int MinuteInterval;

        public SearchSubscriptionService()
        {
            MinuteInterval = 1;
        }

        protected override void OnStart(string[] args)
        {
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
