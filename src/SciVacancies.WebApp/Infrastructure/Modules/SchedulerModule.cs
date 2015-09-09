using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Core;
using Quartz.Impl;
using Quartz.Spi;
using Quartz.Util;
using Autofac;
using Newtonsoft.Json;
using Microsoft.Framework.OptionsModel;
using System.Collections.Specialized;

namespace SciVacancies.WebApp.Infrastructure.Modules
{
    public class SchedulerModule : Module
    {
        readonly IOptions<QuartzSettings> _quartzSettings;

        public SchedulerModule(IOptions<QuartzSettings> quartzSettings)
        {
            this._quartzSettings = quartzSettings;
        }
        protected override void Load(ContainerBuilder builder)
        {
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "ApiBackendScheduler";
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            properties["quartz.jobStore.useProperties"] = "true";
            properties["quartz.jobStore.dataSource"] = "default";
            properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
            properties["quartz.jobStore.lockHandler.type"] = "Quartz.Impl.AdoJobStore.UpdateLockRowSemaphore, Quartz";
            properties["quartz.dataSource.default.connectionString"] = "Data Source=.;Initial Catalog=ApiBackend;Integrated Security=True;Connection Timeout=120;";
            properties["quartz.dataSource.default.provider"] = "SqlServer-20";

            builder.Register(c => new AutofacJobFactory(c.Resolve<ILifetimeScope>())).As<IJobFactory>().SingleInstance();
            builder.Register(c => new AutofacShedulerFactory(c.Resolve<IJobFactory>())).As<ISchedulerExporter>().SingleInstance();
        }
    }

    public class AutofacShedulerFactory : StdSchedulerFactory
    {
        private readonly IJobFactory jobFactory;

        public AutofacShedulerFactory(IJobFactory jobFactory)
        {
            this.jobFactory = jobFactory;
            //this
            
        }

        protected override IScheduler Instantiate(QuartzSchedulerResources rsrcs, QuartzScheduler qs)
        {
            qs.JobFactory = this.jobFactory;
            return base.Instantiate(rsrcs, qs);
        }
    }

    public class AutofacJobFactory : IJobFactory
    {
        private readonly ILifetimeScope lifetimeScope;

        public AutofacJobFactory(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            var jobType = jobDetail.JobType;

            try
            {
                var job = lifetimeScope.Resolve(jobType);

                var jsonData = jobDetail.JobDataMap.GetString("data");

                JsonConvert.PopulateObject(jsonData, job);

                return (IJob)job;
            }
            catch (Exception e)
            {
                var message = String.Format("Problem instantiating class {0}", jobType != null ? jobType.Name : "UNKNOWN");
                throw new SchedulerException(message, e);
            }
        }

        public void ReturnJob(IJob job)
        {

        }
    }
}
