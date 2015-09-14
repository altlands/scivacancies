using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Topshelf;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Quartz.Core;
using SciVacancies.SearchSubscriptionsService.Jobs;

namespace SciVacancies.SearchSubscriptionsService
{
    public class SubscriptionService : ServiceControl
    {
        readonly int MinuteInterval;

        public SubscriptionService()
        {
            Debugger.Break();
            MinuteInterval = 1;
        }

        public bool Start(HostControl hostControl) => Start();
        public bool Start()
        {
            //logging

            //quartz
            try
            {
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler();

                var jobKey = new JobKey("SciVacancies.SearchSubscriptionJob", "SciVacancies.SearchSubscriptionService");
                var triggerKey = new TriggerKey("SciVacancies.SearchSubscriptionJobTrigger", "SciVacancies.SearchSubscriptionService");

                if (scheduler.CheckExists(jobKey) && scheduler.CheckExists(triggerKey))
                {
                    scheduler.DeleteJob(jobKey);
                }

                var job = JobBuilder.Create<SearchSubscriptionJob>()
                                            .WithIdentity(jobKey)
                                            .Build();

                var trigger = TriggerBuilder.Create()
                                                .WithIdentity(triggerKey)
                                                .WithSimpleSchedule(s => s
                                                    .WithIntervalInMinutes(MinuteInterval)
                                                    .RepeatForever())
                                                .Build();

                scheduler.ScheduleJob(job, trigger);

                scheduler.Start();
            }
            catch (Exception e)
            {

            }

            Console.WriteLine("Started");

            return true;
        }

        public bool Stop(HostControl hostControl) => Stop();
        public bool Stop()
        {
            return true;
        }
    }
}
