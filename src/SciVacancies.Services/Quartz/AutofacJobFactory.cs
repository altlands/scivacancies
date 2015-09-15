using System;
using Autofac;
using Newtonsoft.Json;
using Quartz;
using Quartz.Spi;

namespace SciVacancies.Services.Quartz
{
    public class AutofacJobFactory : IJobFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacJobFactory(ILifetimeScope lifetimeScope)
        {
            this._lifetimeScope = lifetimeScope;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            var jobType = jobDetail.JobType;

            try
            {
                var job = _lifetimeScope.Resolve(jobType);

                var jsonData = jobDetail.JobDataMap.GetString("data");

                JsonConvert.PopulateObject(jsonData, job);

                return (IJob)job;
            }
            catch (Exception e)
            {
                var message = string.Format("Problem instantiating class {0}", jobType?.Name ?? "UNKNOWN");
                throw new SchedulerException(message, e);
            }
        }

        public void ReturnJob(IJob job)
        {

        }
    }
}
