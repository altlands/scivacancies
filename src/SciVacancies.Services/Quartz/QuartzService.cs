using System;
using System.Collections.Generic;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Microsoft.Framework.ConfigurationModel;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace SciVacancies.Services.Quartz
{
    public class QuartzService : ISchedulerService
    {
        readonly IScheduler _scheduler;

        public QuartzService(IConfiguration configuration, IJobFactory jobFactory)
        {
            var properties = new NameValueCollection
            {
                ["quartz.scheduler.instanceName"] = configuration.Get("QuartzSettings:Scheduler:InstanceName"),
                ["quartz.jobStore.type"] = configuration.Get("QuartzSettings:JobStore:Type"),
                ["quartz.jobStore.useProperties"] = configuration.Get("QuartzSettings:JobStore:UseProperties"),
                ["quartz.jobStore.dataSource"] = configuration.Get("QuartzSettings:JobStore:DataSource"),
                ["quartz.jobStore.tablePrefix"] = configuration.Get("QuartzSettings:JobStore:TablePrefix"),
                ["quartz.jobStore.lockHandler.type"] = configuration.Get("QuartzSettings:JobStore:LockHandler:Type"),
                ["quartz.dataSource.default.connectionString"] = configuration.Get("QuartzSettings:DataSource:Default:ConnectionString"),
                ["quartz.dataSource.default.provider"] = configuration.Get("QuartzSettings:DataSource:Default:Provider"),
                ["quartz.jobStore.misfireThreshold"] = "60000"
            };

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory(properties);
            _scheduler = schedulerFactory.GetScheduler();
            _scheduler.JobFactory = jobFactory;
        }

        public void CreateSheduledJob<T>(T jobObject, object jobIdentity, DateTime executionTime) where T : IJob
        {
            var jsonData = JsonConvert.SerializeObject(jobObject);

            var jobDetail = JobBuilder.Create<T>()
                .WithIdentity(jobIdentity.ToString(), typeof(T).Name)
                .UsingJobData("data", jsonData)
                .Build();

            var trigger = TriggerBuilder.Create()
                    .WithIdentity(jobIdentity.ToString(), typeof(T).Name)
                    .StartAt(executionTime)
                    .WithSimpleSchedule(x => x.WithMisfireHandlingInstructionFireNow())
                    .Build();

            _scheduler.ScheduleJob(jobDetail, trigger);
        }

        public void CreateSheduledJob<T>(T jobObject, object jobIdentity, int executionInterval) where T : IJob
        {
            var jsonData = JsonConvert.SerializeObject(jobObject);

            var jobDetail = JobBuilder.Create<T>()
                .WithIdentity(jobIdentity.ToString(), typeof(T).Name)
                .UsingJobData("data", jsonData)
                .Build();

            var trigger = TriggerBuilder.Create()
                    .WithIdentity(jobIdentity.ToString(), typeof(T).Name)
                    .WithSimpleSchedule(s => s
                        .WithIntervalInMinutes(executionInterval)
                        .RepeatForever()
                    )
                    .Build();

            _scheduler.ScheduleJob(jobDetail, trigger);
        }

        public void CreateSheduledJobWithStrongName<T>(T jobObject, JobKey jobIdentity, int executionInterval) where T : IJob
        {

            var jsonData = JsonConvert.SerializeObject(jobObject);

            var jobDetail = JobBuilder.Create<T>()
                .WithIdentity(jobIdentity.Name, jobIdentity.Group)
                .UsingJobData("data", jsonData)
                .Build();

            var trigger = TriggerBuilder.Create()
                    .WithIdentity(jobIdentity.Name, jobIdentity.Group)
                    .WithSimpleSchedule(s => s
                        .WithIntervalInMinutes(executionInterval)
                        .RepeatForever()
                    )
                    .Build();

            Console.WriteLine($">>>>>>>>> Создаём Рабочего с интервалом {executionInterval} (минут) ");

            _scheduler.ScheduleJob(jobDetail, trigger);
        }

        public void RemoveScheduledJob(object jobIdentity)
        {
            _scheduler.DeleteJob(new JobKey(jobIdentity.ToString()));
        }

        public void StartScheduler()
        {
            if (!_scheduler.IsStarted) _scheduler.Start();
        }

        public void StopScheduler()
        {
            if (_scheduler.IsStarted) _scheduler.Shutdown(true);
        }

        public bool CheckExists(JobKey jobKey)
        {
            return _scheduler.CheckExists(jobKey);
        }

        public bool CheckExists(TriggerKey triggerKey)
        {
            return _scheduler.CheckExists(triggerKey);
        }

        public IJobDetail GetJobDetail(JobKey jobKey)
        {
            return _scheduler.GetJobDetail(jobKey);
        }

        public IList<ITrigger> GetTriggersOfJob(JobKey jobKey)
        {
            return _scheduler.GetTriggersOfJob(jobKey);
        }

        public bool DeleteJob(JobKey jobKey)
        {
            return _scheduler.DeleteJob(jobKey);
        }

        public void Shutdown()
        {
            _scheduler.Shutdown();
        }
    }
}
