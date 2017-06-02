using SciVacancies.Services.Logging;

using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.OptionsModel;
using Npgsql;
using NPoco;

namespace SciVacancies.WebApp.Infrastructure
{
    public class ReadModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.Register(c => 
                new Database(c.Resolve<IOptions<DbSettings>>().Value.ReadModelDb, NpgsqlFactory.Instance)
            )
            .As<IDatabase>()

            .InstancePerDependency()
            //.InstancePerLifetimeScope()

            .OnRelease(database =>
            {
                database.Connection.Close();
                database.Connection.Dispose();
                database.Dispose();
            })

            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CallLogger))

            ;
        }
    }
}
