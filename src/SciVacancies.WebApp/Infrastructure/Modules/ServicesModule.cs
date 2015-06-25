using Autofac;

using SciVacancies.ReadModel;

using Nest;

namespace SciVacancies.WebApp.Infrastructure
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ElasticFactory()).As<IElasticClient>().SingleInstance();
        }
    }
}