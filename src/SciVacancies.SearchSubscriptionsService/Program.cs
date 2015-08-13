using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace SciVacancies.SearchSubscriptionsService
{
    public class Program
    {
        private readonly int HOUR_INTERVAL = 24;
        private readonly int HOUR_RUN = 10;
        private readonly int MINUTE_RUN = 12;

        public void Main(string[] args)
        {
            try
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

                scheduler.Start();

                IJobDetail job = JobBuilder.Create<SearchSubscriptionJob>()
                    .WithIdentity("SciVacancies.SearchSubscriptionService", "SciVacancies")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("SciVacancies.SearchSubscriptionJobTrigger", "SciVacancies")
                    .WithDailyTimeIntervalSchedule(s => s
                        .WithIntervalInHours(HOUR_INTERVAL)
                        .OnEveryDay()
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(HOUR_RUN, MINUTE_RUN))
                    )
                    .Build();

                scheduler.ScheduleJob(job, trigger);
            }
            catch (SchedulerException e)
            {
                throw;
            }
        }
    }
}
