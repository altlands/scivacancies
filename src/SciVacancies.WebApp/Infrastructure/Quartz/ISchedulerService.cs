using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;

namespace SciVacancies.WebApp.Infrastructure
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
