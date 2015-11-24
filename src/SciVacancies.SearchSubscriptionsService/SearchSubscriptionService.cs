using System;
using System.Linq;
using System.ServiceProcess;
using Autofac;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Logging;
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

        private readonly ILogger Logger;

        public SearchSubscriptionService(IConfiguration configuration, ILifetimeScope lifetimeScope, ILoggerFactory loggerFactory)
        {
            _lifetimeScope = lifetimeScope;
            _schedulerService = _lifetimeScope.Resolve<ISchedulerService>();

            MinuteInterval = configuration.Get<QuartzSettings>("QuartzSettings").Scheduler.ExecutionInterval;
            this.Logger = loggerFactory.CreateLogger<SearchSubscriptionService>();
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

            Logger.LogInformation("Starting quartz scheduler...");
            //quartz
            try
            {
                Logger.LogInformation("Resolving quartz sheduled job");
                var searchSubscriptionJob = _lifetimeScope.Resolve<SearchSubscriptionJob>();
                Logger.LogInformation("Resolved quartz sheduled job");

                //var jobKey = new JobKey("SciVacancies.SearchSubscriptionJob", "SciVacancies.SearchSubscriptionService");
                //var triggerKey = new TriggerKey("SciVacancies.SearchSubscriptionJobTrigger", "SciVacancies.SearchSubscriptionService");


                //if (!_schedulerService.CheckExists(jobKey))
                //{
                //    Logger.LogInformation("Creating quartz sheduled job");
                //    _schedulerService.CreateSheduledJobWithStrongName(searchSubscriptionJob, jobKey, MinuteInterval);
                //    Logger.LogInformation("Sheduled job has been created");
                //}
                //else
                //{
                //    //если job существует, то сравнить параметры его работы с настройками в config'e.

                //    var savedTriggersOfJobs = _schedulerService.GetTriggersOfJob(jobKey);
                //    var triggerHasChanges = savedTriggersOfJobs
                //        .Select(c => c as Quartz.Impl.Triggers.SimpleTriggerImpl)
                //        .Any(c => c.RepeatInterval.Minutes != MinuteInterval);

                //    if (triggerHasChanges)
                //    //recreate trigger
                //    {
                //        Logger.LogInformation("Recreating quartz sheduled job");

                //        _schedulerService.DeleteJob(jobKey);

                //        _schedulerService.CreateSheduledJobWithStrongName(searchSubscriptionJob, jobKey, MinuteInterval);

                //        Logger.LogInformation("Sheduled job has been recreated");
                //    }
                //}

                //_schedulerService.StartScheduler();

                Logger.LogInformation("Starting quartz sheduled job ExecuteJob");
                searchSubscriptionJob.ExecuteJob();
                Logger.LogInformation("Finnished quartz sheduled job ExecuteJob");

            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, e);
            }

            Logger.LogInformation("Quartz scheduler has been started");
        }

        protected override void OnStop()
        {
            Logger.LogInformation("Quartz scheduler is stopping");

            try
            {
                _schedulerService.Shutdown();
                base.OnStop();
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, e);
            }

            Logger.LogInformation("Quartz scheduler has been stopped");
        }
    }
}
