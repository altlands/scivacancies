using System;
using System.ServiceProcess;
using Autofac;
using Microsoft.Framework.ConfigurationModel;
using Quartz;
using SciVacancies.SearchSubscriptionsService.Jobs;
using SciVacancies.Services.Quartz;

namespace SciVacancies.SearchSubscriptionsService
{
    public class SearchSubscriptionService : ServiceBase
    {
        readonly int MinuteInterval;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ISchedulerService _schedulerService;

        public SearchSubscriptionService(IConfiguration configuration, ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            _schedulerService = _lifetimeScope.Resolve<ISchedulerService>();

            MinuteInterval = int.Parse(configuration.Get("QuartzSettings:Scheduler:ExecutionInterval"));
        }

        /// <summary>
        /// используется при запуске в консоли
        /// </summary>
        public void OnStart()
        {
            OnStart(null);
        }
        /// <summary>
        /// используется при запущенном сервисе
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            Console.WriteLine("SearchSubscriptionService Starting");

            //logging

            //quartz
            try
            {
                //ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                //_schedulerService = schedulerFactory.GetScheduler();

                var jobKey = new JobKey("SciVacancies.SearchSubscriptionJob", "SciVacancies.SearchSubscriptionService");
                var triggerKey = new TriggerKey("SciVacancies.SearchSubscriptionJobTrigger", "SciVacancies.SearchSubscriptionService");


                //var job = JobBuilder.Create<SearchSubscriptionJob>()
                //                            .WithIdentity(jobKey)
                //                            .Build();
                //var trigger = TriggerBuilder.Create()
                //                                .WithIdentity(triggerKey)
                //                                .WithSimpleSchedule(s => s
                //                                    .WithIntervalInMinutes(MinuteInterval)
                //                                    .RepeatForever())
                //                                .Build();
                //_schedulerService.ScheduleJob(job, trigger);

                if (!_schedulerService.CheckExists(jobKey))
                {
                    //_schedulerService.DeleteJob(jobKey);
                    _schedulerService.CreateSheduledJobWithStrongName(_lifetimeScope.Resolve<SearchSubscriptionJob>(), jobKey, MinuteInterval);
                }
                

                _schedulerService.StartScheduler();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception happend: {e.Message}");
            }


            Console.WriteLine("SearchSubscriptionService Started");
            Console.WriteLine("");
        }

        protected override void OnStop()
        {
            Console.WriteLine("SearchSubscriptionService Stopping");
            _schedulerService.Shutdown();
            //...???

            base.OnStop();
            Console.WriteLine("SearchSubscriptionService Stopped");
            Console.WriteLine("");
        }
    }
}
