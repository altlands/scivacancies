using System;
using Quartz;

namespace SciVacancies.Services.Quartz
{
    public interface ISchedulerService
    {
        void CreateSheduledJob<T>(T jobObject, object jobIdentity, DateTime executionTime) where T : IJob;
        void CreateSheduledJob<T>(T jobObject, object jobIdentity, int executionInterval) where T : IJob;
        void RemoveScheduledJob(object jobIdentity);
        void StartScheduler();
        void StopScheduler();
    }
}
