using SciVacancies.Services.Logging;
using SciVacancies.Services.Quartz;

using Autofac;
using Autofac.Extras.DynamicProxy;
using Quartz;
using Quartz.Spi;

namespace SciVacancies.WebApp.Infrastructure
{
    public class QuartzModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<QuartzService>()
                .As<ISchedulerService>()
                .SingleInstance()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(CallLogger))
                ;
            builder.RegisterType<AutofacJobFactory>()
                .As<IJobFactory>()
                .InstancePerLifetimeScope()
                ;
            builder.RegisterTypes(System.Reflection.Assembly.GetAssembly(typeof(QuartzModule)).GetTypes())
               .Where(t => t != typeof(IJob) && typeof(IJob).IsAssignableFrom(t))
               .AsSelf()
               .InstancePerLifetimeScope()
               ;
        }
    }
}
