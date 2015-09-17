using System;
using System.Collections.Specialized;
using Microsoft.Framework.ConfigurationModel;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using SciVacancies.Services.Quartz;

namespace SciVacancies.SearchSubscriptionsService.Quartz
{
    public class QuartzService : ISchedulerService
    {
        readonly IConfiguration configuration;
        readonly ISchedulerFactory schedulerFactory;
        readonly IScheduler scheduler;

        public QuartzService(IConfiguration configuration, IJobFactory jobFactory)
        {
            this.configuration = configuration;

            NameValueCollection properties = new NameValueCollection();

            properties["quartz.scheduler.instanceName"] = configuration.Get("QuartzSettings:Scheduler:InstanceName");
            properties["quartz.jobStore.type"] = configuration.Get("QuartzSettings:JobStore:Type");
            properties["quartz.jobStore.useProperties"] = configuration.Get("QuartzSettings:JobStore:UseProperties");
            properties["quartz.jobStore.dataSource"] = configuration.Get("QuartzSettings:JobStore:DataSource");
            properties["quartz.jobStore.tablePrefix"] = configuration.Get("QuartzSettings:JobStore:TablePrefix");
            properties["quartz.jobStore.lockHandler.type"] = configuration.Get("QuartzSettings:JobStore:LockHandler:Type");
            properties["quartz.dataSource.default.connectionString"] = configuration.Get("QuartzSettings:DataSource:Default:ConnectionString");
            properties["quartz.dataSource.default.provider"] = configuration.Get("QuartzSettings:DataSource:Default:Provider");
            properties["quartz.jobStore.misfireThreshold"] = "60000";

            schedulerFactory = new StdSchedulerFactory(properties);
            scheduler = schedulerFactory.GetScheduler();
            scheduler.JobFactory = jobFactory;
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

            scheduler.ScheduleJob(jobDetail, trigger);
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

            scheduler.ScheduleJob(jobDetail, trigger);
        }

        public void RemoveScheduledJob(object jobIdentity)
        {
            scheduler.DeleteJob(new JobKey(jobIdentity.ToString()));
        }

        public void StartScheduler()
        {
            if (!scheduler.IsStarted) scheduler.Start();
        }

        public void StopScheduler()
        {
            if (scheduler.IsStarted) scheduler.Shutdown(true);
        }
    }

}
