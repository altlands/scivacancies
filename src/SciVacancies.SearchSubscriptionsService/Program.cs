using System;
using Autofac;
using Topshelf;

namespace SciVacancies.SearchSubscriptionsService
{
    public class Program
    {
        public int Main(string[] args)
        {
            var result =  (int)HostFactory.Run(x =>
            {
                x.StartAutomatically();
                x.Service(settings => new SubscriptionService());

                if (ServiceConfiguration.UseCredentials)
                {
                    x.RunAs(ServiceConfiguration.ServiceUserName, ServiceConfiguration.ServicePassword);
                }
                else
                {
                    x.RunAsLocalSystem();
                }

                x.SetDescription("SciVacancies.SearchSubscriptionService: выполняет поиск Подписок, их обработку и рассылку уведомлений");
                x.SetDisplayName("SciVacancies.SearchSubscriptionService");
                x.SetServiceName("SciVacancies.SearchSubscriptionService");
            });


            Console.WriteLine(result);


            var line = string.Empty;
            while (line!="exit")
            {
                line = Console.ReadLine();
            }


            return result;
        }
    }
}
