using SciVacancies.Services.Elastic;
using SciVacancies.Services.Logging;

using System;
using Microsoft.Extensions.Logging;

using Nest;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.OptionsModel;

namespace SciVacancies.SearchSubscriptionsService.Modules
{
    public class ServicesModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {

            builder.Register(c => new ElasticClient(
                    new ConnectionSettings(
                            new Uri(c.Resolve<ElasticSettings>().ConnectionUrl),
                            defaultIndex: c.Resolve<ElasticSettings>().DefaultIndex
                        )
                ))
                .As<IElasticClient>()
                ;
            builder.Register(c => new SearchService(
                    c.Resolve<ElasticSettings>().MinScore, 
                    c.Resolve<IElasticClient>(), 
                    c.Resolve<ILoggerFactory>()
                    )
                    )
                .As<ISearchService>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(CallLogger))
                ;

            builder.RegisterType<SearchSubscriptionService>()
                .AsSelf()
                ;

            builder.RegisterType<SearchSubscriptionScanner>()
                .As<ISearchSubscriptionScanner>()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(CallLogger));
        }
    }
}