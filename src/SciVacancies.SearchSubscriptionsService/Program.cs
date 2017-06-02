using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using SciVacancies.SearchSubscriptionsService.Modules;
using SciVacancies.Services;


namespace SciVacancies.SearchSubscriptionsService
{
    public class Program
    {
        private string devEnv { get; }
        public IConfiguration Configuration { get; set; }
        public IContainer Container { get; set; }
        private readonly ILoggerFactory _loggerFactory;

        public Program(IApplicationEnvironment appEnv)
        {
            var vars = Environment.GetEnvironmentVariables();
            devEnv = (string)vars.Cast<DictionaryEntry>().FirstOrDefault(e => e.Key.Equals("dev_env")).Value;

            IConfigurationBuilder configurationBuilder;

            var useNonTemplateConfigFile = new UpdateConfigService().VerifyIfChangeEnvInFile(vars, PlatformServices.Default.Application.ApplicationBasePath, Path.DirectorySeparatorChar, devEnv, new Dictionary<string, string>
            {
                {"Db_IP", "SUBSPOSTGRESALIAS_PORT_5432_TCP_ADDR"},
                {"Db_PORT", "SUBSPOSTGRESALIAS_PORT_5432_TCP_PORT"},
                {"ElasticSearch_IP", "SUBSELASTICSEARCHALIAS_PORT_9200_TCP_ADDR"},
                {"ElasticSearch_PORT", "SUBSELASTICSEARCHALIAS_PORT_9200_TCP_PORT"},
                {"Host_Out_Adress", "HOST_IP"}
            });

            if (String.IsNullOrEmpty(devEnv))
            {
                if (useNonTemplateConfigFile)
                    configurationBuilder = new ConfigurationBuilder()
                        .SetBasePath(appEnv.ApplicationBasePath)
                        .AddJsonFile("config.modified.json")
                        .AddEnvironmentVariables();
                else
                    configurationBuilder = new ConfigurationBuilder()
                        .SetBasePath(appEnv.ApplicationBasePath)
                        .AddJsonFile("config.json")
                        .AddEnvironmentVariables();
            }
            else
            {
                if (useNonTemplateConfigFile)
                    configurationBuilder = new ConfigurationBuilder()
                         .SetBasePath(appEnv.ApplicationBasePath)
                         .AddJsonFile($"config.{devEnv}.modified.json", optional: false)
                         .AddEnvironmentVariables();
                else
                    configurationBuilder = new ConfigurationBuilder()
                         .SetBasePath(appEnv.ApplicationBasePath)
                         .AddJsonFile($"config.{devEnv}.json", optional: false)
                         .AddEnvironmentVariables();
            }


            Configuration = configurationBuilder.Build();

            //DI to Controllers, http-requests, etc.
            _loggerFactory = new LoggerFactory();
            _loggerFactory.AddSerilog(LoggingModule.LoggerConfiguration(Configuration).CreateLogger());


            var builder = new ContainerBuilder();

            builder.Register(c => _loggerFactory)
                .As<ILoggerFactory>()
                .SingleInstance();

            var dataSettings = Configuration.Get<DataSettings>("Data");
            var quartzSettings = Configuration.Get<QuartzSettings>("QuartzSettings");
            var elasticSettings = Configuration.Get<ElasticSettings>("ElasticSettings");
          
            builder.Register(c => dataSettings)
                .As<DataSettings>()
                .SingleInstance();
            builder.Register(c => quartzSettings)
                .As<QuartzSettings>()
                .SingleInstance();
            builder.Register(c => elasticSettings)
                .As<ElasticSettings>()
                .SingleInstance();


            builder.Register(c => Configuration).As<IConfiguration>().SingleInstance();
            builder.RegisterModule(new ReadModelModule());
            builder.RegisterModule(new ServicesModule());
            builder.RegisterModule(new QuartzModule());
            builder.RegisterModule(new SmtpNotificationModule());
            builder.RegisterModule(new LoggingModule());



            Container = builder.Build();
        }


        public void Main(string[] args)
        {

            ILoggerFactory loggerFactory = Container.Resolve<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Resolving from container");

            var service = Container.Resolve<SearchSubscriptionService>();
            try
            {
                service.OnStart();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
            }

            logger.LogInformation("Service is stopping");
            service.Stop();
            logger.LogInformation("Service has been stopped");
        }
    }


}
