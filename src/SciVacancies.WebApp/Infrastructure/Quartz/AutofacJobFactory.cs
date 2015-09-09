using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Core;
using Quartz.Impl;
using Quartz.Spi;
using Autofac;
using Newtonsoft.Json;

namespace SciVacancies.WebApp.Infrastructure
{
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
