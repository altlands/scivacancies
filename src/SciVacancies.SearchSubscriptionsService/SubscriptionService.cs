using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Topshelf;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Quartz.Core;

namespace SciVacancies.SearchSubscriptionsService
{
    public class SubscriptionService : ServiceControl
    {
        readonly int MinuteInterval;

        public SubscriptionService()
        {
            MinuteInterval = 1;
        }

        public bool Start(HostControl hostControl)
        {
            //logging

            //quartz
            try
            {
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                IScheduler scheduler = schedulerFactory.GetScheduler();

                JobKey jobKey = new JobKey("SciVacancies.SearchSubscriptionJob", "SciVacancies.SearchSubscriptionService");
                TriggerKey triggerKey = new TriggerKey("SciVacancies.SearchSubscriptionJobTrigger", "SciVacancies.SearchSubscriptionService");

                if (scheduler.CheckExists(jobKey) && scheduler.CheckExists(triggerKey))
                {
                    scheduler.DeleteJob(jobKey);
                }

                IJobDetail job = JobBuilder.Create<SearchSubscriptionJob>()
                                            .WithIdentity(jobKey)
                                            .Build();

                ITrigger trigger = TriggerBuilder.Create()
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

            return true;
        }
        public bool Stop(HostControl hostControl)
        {
            return true;
        }
    }
}
