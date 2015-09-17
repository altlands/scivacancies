namespace SciVacancies.WebApp
{
    public class QuartzSettings
    {
        public QuartzSchedulerSettings Scheduler { get; set; }
        public QuartzJobStoreSettings JobStore { get; set; }
        public QuartzDataSourceSettings DataSource { get; set; }
    }

    public class QuartzSchedulerSettings
    {
        public string InstanceName { get; set; }
        public int ExecutionInterval { get; set; }
    }
    public class QuartzJobStoreSettings
    {
        public string Type { get; set; }
        public string UseProperties { get; set; }
        public string DataSource { get; set; }
        public string TablePrefix { get; set; }
        public JobStoreLockHandlerSettings LockHandler { get; set; }
    }
    public class JobStoreLockHandlerSettings
    {
        public string Type { get; set; }
    }

    public class QuartzDataSourceSettings
    {
        public DefaultDataSourceSettings Default { get; set; }
    }
    public class DefaultDataSourceSettings
    {
        public string ConnectionStrings { get; set; }
        public string Provider { get; set; }
    }
}
