using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Spi;
using Autofac;

namespace SciVacancies.WebApp.Infrastructure
{
    public class QuartzModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<QuartzService>().AsImplementedInterfaces();
            builder.RegisterType<AutofacJobFactory>().As<IJobFactory>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(new System.Reflection.Assembly[]
            {
                System.Reflection.Assembly.Load("SciVacancies.WebApp")
            })
                .AsClosedTypesOf(typeof(IJob))
                .AsImplementedInterfaces();
        }
    }
}
