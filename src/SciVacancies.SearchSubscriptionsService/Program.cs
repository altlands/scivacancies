using System;
using System.Collections;
using System.Linq;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using SciVacancies.SearchSubscriptionsService.Modules;

//using Microsoft.rd

namespace SciVacancies.SearchSubscriptionsService
{
    public class Program
    {
        public IConfiguration Configuration { get; set; }

        public IContainer Container { get; set; }

        private string devEnv { get; }

        private ILogger Logger { get; }

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
            Logger = loggerFactory.CreateLogger<Program>();
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

            var service = Container.Resolve<SearchSubscriptionService>();
            try
            {
                service.OnStart();
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, e);
            }

            //var wroteCommand = Console.ReadLine();
            //while (wroteCommand == null || !wroteCommand.Equals("stop"))
            //{
            //    Thread.Sleep(2000);
            //    wroteCommand = Console.ReadLine();
            //}

            //while (!service.CanStop)
            //{
            //    Thread.Sleep(500);
            //}

            Logger.LogInformation("Service is stopping");
            service.Stop();
            Logger.LogInformation("Service has been stopped");
        }
    }
}
