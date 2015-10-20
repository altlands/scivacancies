using System;
using System.Linq;
using System.Collections;
using System.ServiceProcess;
using System.Threading;
using Autofac;
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

namespace SciVacancies.SearchSubscriptionsService
{
    public class Program
    {
        public void Main(string[] args)
        {
            var vars = Environment.GetEnvironmentVariables();
            var devEnv = (string)vars.Cast<DictionaryEntry>().FirstOrDefault(e => e.Key.Equals("dev_env")).Value;
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{devEnv}.json", optional: true)
                .AddEnvironmentVariables()
                .Build()
                ;
            
            var builder = new ContainerBuilder();
            builder.Register(c => configuration).As<IConfiguration>().SingleInstance();
            builder.RegisterType<SmtpNotificatorService>().As<ISmtpNotificatorService>().InstancePerDependency();
            builder.RegisterType<SearchSubscriptionService>().AsSelf();
            builder.RegisterType<QuartzService>().AsImplementedInterfaces();

            ConnectionSettings elasticConnectionSettings = new ConnectionSettings(new Uri(configuration.Get<ElasticSettings>("ElasticSettings").ConnectionUrl), defaultIndex: configuration.Get<ElasticSettings>("ElasticSettings").DefaultIndex);
            builder.Register(c => new ElasticClient(elasticConnectionSettings)).As<IElasticClient>().InstancePerDependency();
            //TODO single instanse or not?
            builder.Register(c => new SearchService(configuration, c.Resolve<IElasticClient>())).As<IElasticService>().InstancePerDependency();

            builder.RegisterType<AutofacJobFactory>().As<IJobFactory>().InstancePerLifetimeScope();
            builder.RegisterType<SearchSubscriptionJob>().As<IJob>().InstancePerLifetimeScope().AsSelf();
            builder.RegisterTypes(System.Reflection.Assembly.GetAssembly(typeof(Program)).GetTypes())
                .Where(t => t != typeof(IJob) && typeof(IJob).IsAssignableFrom(t))
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterType<SearchSubscriptionScanner>().As<ISearchSubscriptionScanner>().InstancePerDependency();
            builder.Register(c => new Database(configuration.Get<DataSettings>("Data").ReadModelDb, NpgsqlFactory.Instance)).As<IDatabase>().InstancePerDependency();
            //builder.Register(c => new Database(Config.Get("Data:ReadModelDb"), NpgsqlFactory.Instance)).As<IDatabase>().InstancePerLifetimeScope();

            var container = builder.Build();

            //HostFactory.Run(hostConfigurator =>
            //{
            //    hostConfigurator.StartAutomatically();
            //    hostConfigurator.Service<SearchSubscriptionService>(serviceConfigurator =>
            //    {
            //        serviceConfigurator.ConstructUsing(() => new SearchSubscriptionService());
            //        serviceConfigurator.WhenStarted((service, hostControl) => service.Start(hostControl));
            //        serviceConfigurator.WhenStarted((service, hostControl) => service.Stop(hostControl));
            //    });

            //    if (ServiceConfiguration.UseCredentials)
            //    {
            //        hostConfigurator.RunAs(ServiceConfiguration.ServiceUserName, ServiceConfiguration.ServicePassword);
            //    }
            //    else
            //    {
            //        hostConfigurator.RunAsLocalSystem();
            //    }

            //    hostConfigurator.SetDescription("SciVacancies.SearchSubscriptionService: выполняет поиск Подписок, их обработку и рассылку уведомлений");
            //    hostConfigurator.SetDisplayName("SciVacancies.SearchSubscriptionService");
            //    hostConfigurator.SetServiceName("SciVacancies.SearchSubscriptionService");
            //});

            Console.WriteLine("Started");

            SearchSubscriptionService searchSubscriptionService;
            //service initializing
            try
            {
                searchSubscriptionService = container.Resolve<SearchSubscriptionService>();
                Console.WriteLine("Program: SearchSubscriptionService Resolved");
                searchSubscriptionService.OnStart();
                Console.WriteLine("Program: SearchSubscriptionService Started");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Program:" + exception.Message);
                Console.ReadLine();
                return;
            }
            //searchSubscriptionService.ServiceName = "SearchSubscriptionService";
            //ServiceBase.Run(searchSubscriptionService);

            var wroteCommand = Console.ReadLine();
            while (wroteCommand == null || !wroteCommand.Equals("stop"))
            {
                Thread.Sleep(2000);
                wroteCommand = Console.ReadLine();
            }

            while (!searchSubscriptionService.CanStop)
            {
                Thread.Sleep(500);
            }

            Console.WriteLine("Program: Stopping");
            searchSubscriptionService.Stop();
            Console.WriteLine("Program: SearchSubscriptionService Stopped");
            Console.ReadLine();

        }
    }
}
