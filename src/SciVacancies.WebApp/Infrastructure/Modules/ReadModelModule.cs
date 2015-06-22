using Autofac;
using Microsoft.Framework.ConfigurationModel;
using Npgsql;
using NPoco;
using SciVacancies.ReadModel;

namespace SciVacancies.WebApp.Infrastructure
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
            //builder.RegisterType<StubReadModelService>().As<IReadModelService>().SingleInstance();
            builder.RegisterType<ReadModelService>().As<IReadModelService>().SingleInstance();

            builder.Register(c => new Database(Config.Get("Data:ReadModelDb"), NpgsqlFactory.Instance))
            .As<IDatabase>()
                .AsSelf()
                .InstancePerRequest()
                .InstancePerLifetimeScope();
                //.OnActivating(d => d.Instance.BeginTransaction())
                //.OnRelease(d => d.CompleteTransaction());
        }
    }
}
