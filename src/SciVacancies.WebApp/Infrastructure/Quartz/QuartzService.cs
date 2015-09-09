using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Core;
using Quartz.Impl;
using Quartz.Listener;
using Quartz.Spi;
using Microsoft.Framework.ConfigurationModel;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace SciVacancies.WebApp.Infrastructure
{
    public class QuartzService : ISchedulerService
    {
        readonly IConfiguration configuration;
        readonly ISchedulerFactory schedulerFactory;
        readonly IScheduler scheduler;

        public QuartzService(IConfiguration configuration, IJobFactory jobFactory)
        {
            this.configuration = configuration;

            QuartzSettings settings = configuration.Get<QuartzSettings>("QuartzSettigs");

            NameValueCollection properties = new NameValueCollection();

            properties["quartz.scheduler.instanceName"] = settings.Scheduler.InstanceName;
            properties["quartz.jobStore.type"] = settings.JobStore.Type;
            properties["quartz.jobStore.useProperties"] = settings.JobStore.UseProperties;
            properties["quartz.jobStore.dataSource"] = settings.JobStore.DataSource;
            properties["quartz.jobStore.tablePrefix"] = settings.JobStore.TablePrefix;
            properties["quartz.jobStore.lockHandler.type"] = settings.JobStore.LockHandler.Type;
            properties["quartz.dataSource.default.connectionString"] = settings.DataSource.Default.ConnectionString;
            properties["quartz.dataSource.default.provider"] = settings.DataSource.Default.Provider;

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
        //TODO
        //public void ModifyScheduledJob<T>

        //public void RemoveScheduledJob<T>(T jobObject, object jobIdentity) where T :IJob
        //{

        //}

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
