using System;
using System.ServiceProcess;
using Autofac;
using SciVacancies.Services.Quartz;

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

        public void OnStart()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            //base.OnStart(args);

            //logging

            //quartz
            try
            {

                //ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                //var scheduler = schedulerFactory.GetScheduler();

                //var jobKey = new JobKey("SciVacancies.SearchSubscriptionJob", "SciVacancies.SearchSubscriptionService");
                //var triggerKey = new TriggerKey("SciVacancies.SearchSubscriptionJobTrigger", "SciVacancies.SearchSubscriptionService");

                //if (scheduler.CheckExists(jobKey) && scheduler.CheckExists(triggerKey))
                //{
                //    scheduler.DeleteJob(jobKey);
                //}

                //var job = JobBuilder.Create<SearchSubscriptionJob>()
                //                            .WithIdentity(jobKey)
                //                            .Build();

                //var trigger = TriggerBuilder.Create()
                //                                .WithIdentity(triggerKey)
                //                                .WithSimpleSchedule(s => s
                //                                    .WithIntervalInMinutes(MinuteInterval)
                //                                    .RepeatForever())
                //                                .Build();
                //scheduler.ScheduleJob(job, trigger);
                //scheduler.Start();

                var schedulerService = _lifetimeScope.Resolve<ISchedulerService>();
                schedulerService.StartScheduler();

            }
            catch (Exception e)
            {

            }

        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}
