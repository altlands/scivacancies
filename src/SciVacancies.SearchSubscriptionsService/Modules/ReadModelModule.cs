using SciVacancies.Services.Logging;

using Microsoft.Framework.Configuration;

using Autofac;
using Autofac.Extras.DynamicProxy;
using Npgsql;
using NPoco;

namespace SciVacancies.SearchSubscriptionsService.Modules
{
    public class ReadModelModule : Module
    {
        public IConfiguration Config { get; set; }

        public ReadModelModule(IConfiguration cfg)
        {
            Config = cfg;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new Database(Config["Data:ReadModelDb"], NpgsqlFactory.Instance))
            .As<IDatabase>()
            .InstancePerDependency()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CallLogger))
            ;
        }
    }
}
