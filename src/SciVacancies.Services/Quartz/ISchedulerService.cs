using System;
using System.Collections.Generic;
using Quartz;

namespace SciVacancies.Services.Quartz
{
    public interface ISchedulerService
    {
        void CreateSheduledJob<T>(T jobObject, object jobIdentity, DateTime executionTime) where T : IJob;
        void CreateSheduledJob<T>(T jobObject, object jobIdentity, int executionInterval) where T : IJob;
        void CreateSheduledJobWithStrongName<T>(T jobObject, JobKey jobIdentity, int executionInterval) where T : IJob;
        void RemoveScheduledJob(object jobIdentity);
        void StartScheduler();
        void StopScheduler();
        bool CheckExists(JobKey jobKey);
        bool CheckExists(TriggerKey triggerKey);
        IJobDetail GetJobDetail(JobKey jobKey);
        IList<ITrigger> GetTriggersOfJob(JobKey jobKey);
        bool DeleteJob(JobKey jobKey);
        void Shutdown();
    }
}
