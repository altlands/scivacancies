using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Framework.ConfigurationModel;

namespace SciVacancies.WebApp.Infrastructure
{
    public class CompositionRoot
    {
        public ContainerBuilder Builder { get; set; } = new ContainerBuilder();
        public Lazy<IContainer> Container => new Lazy<IContainer>(() => Builder.Build());

        public CompositionRoot(IConfiguration configuration)
        {
            Builder.RegisterModule(new EventStoreModule(configuration));
            Builder.RegisterModule(new EventBusModule());
            Builder.RegisterModule(new EventHandlersModule());
            Builder.RegisterModule(new ReadModelModule(configuration));
            Builder.RegisterModule(new ServicesModule());
            Builder.RegisterModule(new IdentityModule());
        }
    }
}
