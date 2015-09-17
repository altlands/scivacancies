using System;
using System.ServiceProcess;
using System.Threading;
using Autofac;
using Npgsql;
using NPoco;
using SciVacancies.Services.Email;

namespace SciVacancies.SearchSubscriptionsService
{
    public class Program
    {
        public void Main(string[] args)
        {
            //todo: use Quartz


            var builder = new ContainerBuilder();
            builder.RegisterType<SearchSubscriptionService>().AsSelf();
            builder.RegisterType<SearchSubscriptionManager>().As<ISearchSubscriptionManager>().InstancePerLifetimeScope();
            builder.RegisterType<SearchSubscriptionScanner>().As<ISearchSubscriptionScanner>().InstancePerLifetimeScope();
            builder.RegisterType<SmtpNotificatorService>().As<ISmtpNotificatorService>().InstancePerLifetimeScope();
            

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


            //service initializing
            var searchSubscriptionService = container.Resolve<SearchSubscriptionService>();
            searchSubscriptionService.OnStart();
            //searchSubscriptionService.ServiceName = "SearchSubscriptionService";
            //ServiceBase.Run(searchSubscriptionService);

            Console.WriteLine("Started");
            Console.ReadLine();

            while (!searchSubscriptionService.CanStop)
            {
                Thread.Sleep(500);
            }

            searchSubscriptionService.Stop();
        }
    }
}
