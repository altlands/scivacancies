using System;
using System.Linq;
using System.Collections;
using System.ServiceProcess;
using System.Threading;
using Autofac;
using Autofac.Framework.DependencyInjection;
using Nest;
using Npgsql;
using NPoco;
using Quartz;
using Quartz.Spi;
using SciVacancies.SearchSubscriptionsService.Jobs;
using SciVacancies.Services.Email;
using SciVacancies.Services.Quartz;
using SciVacancies.Services.Elastic;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Logging;
using Microsoft.Framework.DependencyInjection;
//using Microsoft.rd
using SciVacancies.SearchSubscriptionsService.Modules;
using Microsoft.Dnx.Runtime;

using Serilog;

namespace SciVacancies.SearchSubscriptionsService
{
    public class Program
    {
        public IConfiguration Configuration { get; set; }

        public IContainer Container { get; set; }

        private string devEnv { get; set; }

        private Microsoft.Framework.Logging.ILogger Logger { get; set; }

        public Program(IApplicationEnvironment appEnv)
        {
            var vars = Environment.GetEnvironmentVariables();
            devEnv = (string)vars.Cast<DictionaryEntry>().FirstOrDefault(e => e.Key.Equals("dev_env")).Value;

            IConfigurationBuilder configurationBuilder;

            if (String.IsNullOrEmpty(devEnv))
            {
                configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(appEnv.ApplicationBasePath)
                    .AddJsonFile("config.json")
                    .AddEnvironmentVariables();
            }
            else
            {
                configurationBuilder = new ConfigurationBuilder()
                     .SetBasePath(appEnv.ApplicationBasePath)
                     .AddJsonFile($"config.{devEnv}.json", optional: false)
                     .AddEnvironmentVariables();
            }

            Configuration = configurationBuilder.Build();

            var builder = new ContainerBuilder();
            ConfigureContainer(builder);
            Container = builder.Build();

            ILoggerFactory loggerFactory = Container.Resolve<ILoggerFactory>();
            this.Logger = loggerFactory.CreateLogger<Program>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register(c => Configuration).As<IConfiguration>().SingleInstance();

            builder.RegisterModule(new ReadModelModule(Configuration));
            builder.RegisterModule(new ServicesModule(Configuration));
            builder.RegisterModule(new QuartzModule());
            builder.RegisterModule(new SmtpNotificationModule());
            builder.RegisterModule(new LoggingModule(Configuration));
        }

        public void Main(string[] args)
        {
            Logger.LogInformation("Resolving from container");

            SearchSubscriptionService service;
            service = Container.Resolve<SearchSubscriptionService>();
            try
            {
                service.OnStart();
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, e);
            }

            var wroteCommand = Console.ReadLine();
            while (wroteCommand == null || !wroteCommand.Equals("stop"))
            {
                Thread.Sleep(2000);
                wroteCommand = Console.ReadLine();
            }

            while (!service.CanStop)
            {
                Thread.Sleep(500);
            }

            Logger.LogInformation("Service is stopping");
            service.Stop();
            Logger.LogInformation("Service has been stopped");
        }
    }
}
