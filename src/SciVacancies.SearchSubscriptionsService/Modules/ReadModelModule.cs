using SciVacancies.Services.Logging;

using Autofac;
using Autofac.Extras.DynamicProxy;
using Npgsql;
using NPoco;

namespace SciVacancies.SearchSubscriptionsService.Modules
{
    public class ReadModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new Database(c.Resolve<DataSettings>().ReadModelDb, NpgsqlFactory.Instance))
            .As<IDatabase>()
            .InstancePerDependency()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CallLogger))
            ;
        }
    }
}
