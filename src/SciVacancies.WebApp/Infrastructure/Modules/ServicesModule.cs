using SciVacancies.Services.Elastic;
using SciVacancies.Services.Logging;

using System;
using Microsoft.Extensions.Logging;

using Nest;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.OptionsModel;

namespace SciVacancies.WebApp.Infrastructure
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                new ElasticClient(
                    new ConnectionSettings(
                            new Uri(c.Resolve<IOptions<ElasticSettings>>().Value.ConnectionUrl),
                            defaultIndex: c.Resolve<IOptions<ElasticSettings>>().Value.DefaultIndex
                        )
                ))
                .As<IElasticClient>()
                ;
            builder.Register(c => new SearchService(
                c.Resolve<IOptions<ElasticSettings>>().Value.MinScore, 
                c.Resolve<IElasticClient>(), 
                c.Resolve<ILoggerFactory>()
                ))
                .As<ISearchService>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(CallLogger))
                ;
            builder.Register(c => new AnalythicService(c.Resolve<IElasticClient>(), c.Resolve<ILoggerFactory>()))
                .As<IAnalythicService>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(CallLogger))
                ;
        }
    }
}