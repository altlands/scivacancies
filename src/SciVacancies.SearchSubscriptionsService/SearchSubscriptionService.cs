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

        private readonly ILogger _logger;

        public SearchSubscriptionService(IConfiguration configuration, ILifetimeScope lifetimeScope, ILoggerFactory loggerFactory)
        {
            _lifetimeScope = lifetimeScope;
            _schedulerService = _lifetimeScope.Resolve<ISchedulerService>();

            MinuteInterval = configuration.Get<QuartzSettings>("QuartzSettings").Scheduler.ExecutionInterval;
            _logger = loggerFactory.CreateLogger<SearchSubscriptionService>();
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

            _logger.LogInformation("Starting quartz scheduler...");
            //quartz
            try
            {
                _logger.LogInformation("Resolving quartz sheduled job");
                var searchSubscriptionJob = _lifetimeScope.Resolve<SearchSubscriptionJob>();
                _logger.LogInformation("Resolved quartz sheduled job");

                //var jobKey = new JobKey("SciVacancies.SearchSubscriptionJob", "SciVacancies.SearchSubscriptionService");
                //var triggerKey = new TriggerKey("SciVacancies.SearchSubscriptionJobTrigger", "SciVacancies.SearchSubscriptionService");


                //if (!_schedulerService.CheckExists(jobKey))
                //{
                //    _logger.LogInformation("Creating quartz sheduled job");
                //    _schedulerService.CreateSheduledJobWithStrongName(searchSubscriptionJob, jobKey, MinuteInterval);
                //    _logger.LogInformation("Sheduled job has been created");
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
                //        _logger.LogInformation("Recreating quartz sheduled job");

                //        _schedulerService.DeleteJob(jobKey);

                //        _schedulerService.CreateSheduledJobWithStrongName(searchSubscriptionJob, jobKey, MinuteInterval);

                //        _logger.LogInformation("Sheduled job has been recreated");
                //    }
                //}

                //_schedulerService.StartScheduler();

                _logger.LogInformation("Starting quartz sheduled job ExecuteJob");
                searchSubscriptionJob.ExecuteJob();
                _logger.LogInformation("Finnished quartz sheduled job ExecuteJob");

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            _logger.LogInformation("Quartz scheduler has been started");
        }

        protected override void OnStop()
        {
            _logger.LogInformation("Quartz scheduler is stopping");

            try
            {
                _schedulerService.Shutdown();
                base.OnStop();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            _logger.LogInformation("Quartz scheduler has been stopped");
        }
    }
}
