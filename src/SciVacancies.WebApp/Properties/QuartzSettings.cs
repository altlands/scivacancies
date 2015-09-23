namespace SciVacancies.WebApp
{
    public class QuartzSettings
    {
        public QuartzSchedulerSettings Scheduler { get; set; } = new QuartzSchedulerSettings();
        public QuartzJobStoreSettings JobStore { get; set; } = new QuartzJobStoreSettings();
        public QuartzDataSourceSettings DataSource { get; set; } = new QuartzDataSourceSettings();
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
        public JobStoreLockHandlerSettings LockHandler { get; set; } = new JobStoreLockHandlerSettings();
    }
    public class JobStoreLockHandlerSettings
    {
        public string Type { get; set; }
    }

    public class QuartzDataSourceSettings
    {
        public DefaultDataSourceSettings Default { get; set; } = new DefaultDataSourceSettings();
    }
    public class DefaultDataSourceSettings
    {
        public string ConnectionString { get; set; }
        public string Provider { get; set; }
    }
}
