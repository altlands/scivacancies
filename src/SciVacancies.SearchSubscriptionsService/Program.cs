using System;
using Topshelf;

namespace SciVacancies.SearchSubscriptionsService
{
    public class Program
    {
        public int Main(string[] args)
        {
            return (int)HostFactory.Run(x =>
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

                x.SetDescription("SciVacancies.SearchSubscriptionService");
                x.SetDisplayName("SciVacancies.SearchSubscriptionService");
                x.SetServiceName("SciVacancies.SearchSubscriptionService");
            });
        }
    }
}
