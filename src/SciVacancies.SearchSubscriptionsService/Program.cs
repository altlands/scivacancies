using System;
using Autofac;
using Autofac.Features.Variance;
using SciVacancies.SearchSubscriptionsService.Quartz;
using SciVacancies.Services.Quartz;
using Topshelf;

namespace SciVacancies.SearchSubscriptionsService
{
    public class Program
    {
        public void Main(string[] args)
        {
            //HostFactory.Run(hostConfigurator =>
            //{
            //    hostConfigurator.StartAutomatically();
            //    hostConfigurator.Service<SubscriptionService>(serviceConfigurator =>
            //    {
            //        serviceConfigurator.ConstructUsing(() => new SubscriptionService());
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


            #region Autofac
            //var builder = new ContainerBuilder();
            //builder.RegisterSource(new ContravariantRegistrationSource());
            //builder.RegisterModule(new QuartzModule());
            ////builder.Populate(services);
            //builder.RegisterType<>()
            //var container = builder.Build();

            //SchedulerServiceInitialize(container.Resolve<ISchedulerService>());

            //return container.Resolve<IServiceProvider>();

            #endregion
            var subscriptionService = new SubscriptionService();
            subscriptionService.Start();

            var line = string.Empty;
            while (line!="exit")
            {
                line = Console.ReadLine();
            }

            subscriptionService.Stop();

        }
    }
}
