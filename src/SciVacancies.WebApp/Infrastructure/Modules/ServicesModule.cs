using SciVacancies.Services.Elastic;
//using SciVacancies.Services.Logging;

using System;
using Microsoft.Framework.Configuration;

using Nest;
using Autofac;
//using Autofac.Extras.DynamicProxy;

namespace SciVacancies.WebApp.Infrastructure
{
    public class ServicesModule : Module
    {
        private readonly IConfiguration _configuration;

        public ServicesModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            ConnectionSettings elasticConnectionSettings = new ConnectionSettings(new Uri(_configuration["ElasticSettings:ConnectionUrl"]), defaultIndex: _configuration["ElasticSettings:DefaultIndex"]);

            builder.Register(c => new ElasticClient(elasticConnectionSettings))
                .As<IElasticClient>()
                .SingleInstance()
                //.EnableInterfaceInterceptors()
                //.InterceptedBy(typeof(CallLogger))
                ;
            builder.Register(c => new SearchService(_configuration, c.Resolve<IElasticClient>()))
                .As<IElasticService>()
                //.EnableInterfaceInterceptors()
                //.InterceptedBy(typeof(CallLogger))
                ;
        }
    }
}