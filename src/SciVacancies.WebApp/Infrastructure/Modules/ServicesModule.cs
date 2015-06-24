using Autofac;

using SciVacancies.ReadModel;

using Nest;

namespace SciVacancies.WebApp.Infrastructure
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ElasticService>().As<IElasticService>().SingleInstance();
            //builder.Register(c => new AggregateFactory()).As<IConstructAggregates>().SingleInstance();
            builder.Register(c => new ElasticFactory()).As<IElasticClient>().SingleInstance();
        }
    }
}