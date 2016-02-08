using SciVacancies.Services.Logging;

using Microsoft.Extensions.Logging;

using Autofac;

namespace SciVacancies.WebApp.Infrastructure
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new CallLogger(c.Resolve<ILoggerFactory>()))
                .InstancePerDependency();
        }
    }
}
