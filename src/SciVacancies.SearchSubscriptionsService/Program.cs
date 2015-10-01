using System;
using System.Linq;
using System.Collections;
using System.ServiceProcess;
using System.Threading;
using Autofac;
using Microsoft.Framework.ConfigurationModel;
using Npgsql;
using NPoco;
using Quartz;
using Quartz.Spi;
using SciVacancies.SearchSubscriptionsService.Jobs;
using SciVacancies.Services.Email;
using SciVacancies.Services.Quartz;

namespace SciVacancies.SearchSubscriptionsService
{
    public class Program
    {
        public void Main(string[] args)
        {
            var vars = Environment.GetEnvironmentVariables();
            var devEnv = (string)vars.Cast<DictionaryEntry>().FirstOrDefault(e => e.Key.Equals("dev_env")).Value;

            var builder = new ContainerBuilder();
            builder
                .Register(c =>
                    new Configuration()
                    .AddJsonFile("config.json")
                    .AddJsonFile($"config.{devEnv}.json", optional: true)
                    .AddEnvironmentVariables())
                .As<IConfiguration>().SingleInstance();
            builder.RegisterType<SmtpNotificatorService>().As<ISmtpNotificatorService>().InstancePerLifetimeScope();
            builder.RegisterType<SearchSubscriptionService>().AsSelf();
            builder.RegisterType<QuartzService>().AsImplementedInterfaces();
            builder.RegisterType<AutofacJobFactory>().As<IJobFactory>().InstancePerLifetimeScope();
            builder.RegisterType<SearchSubscriptionJob>().As<IJob>().InstancePerLifetimeScope().AsSelf();
            builder.RegisterTypes(System.Reflection.Assembly.GetAssembly(typeof(Program)).GetTypes())
                .Where(t => t != typeof(IJob) && typeof(IJob).IsAssignableFrom(t))
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterType<SearchSubscriptionScanner>().As<ISearchSubscriptionScanner>().InstancePerLifetimeScope();
            //todo: move to config
            builder.Register(c => new Database("Server=localhost;Database=scivacancies;User Id=postgres;Password=postgres", NpgsqlFactory.Instance)).As<IDatabase>();
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

            SearchSubscriptionService searchSubscriptionService = null;
            //service initializing
            try
            {
                searchSubscriptionService = container.Resolve<SearchSubscriptionService>();
                searchSubscriptionService.OnStart();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.Read();
                return;
            }
            //searchSubscriptionService.ServiceName = "SearchSubscriptionService";
            //ServiceBase.Run(searchSubscriptionService);

            Console.ReadLine();

            while (!searchSubscriptionService.CanStop)
            {
                Thread.Sleep(500);
            }

            Console.WriteLine("Stopped");
            searchSubscriptionService.Stop();
            Console.Read();

        }
    }
}
