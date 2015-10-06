using SciVacancies.Services.Elastic;

using System;

using Nest;
using Autofac;
using Microsoft.Framework.ConfigurationModel;

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
            ConnectionSettings elasticConnectionSettings = new ConnectionSettings(new Uri(_configuration.Get("ElasticSettings:ConnectionUrl")), defaultIndex: _configuration.Get("ElasticSettings:DefaultIndex"));

            builder.Register(c => new ElasticClient(elasticConnectionSettings)).As<IElasticClient>().SingleInstance();
            //TODO single instanse or not?
            builder.Register(c => new SearchService(_configuration, c.Resolve<IElasticClient>())).As<IElasticService>();
        }
    }
}