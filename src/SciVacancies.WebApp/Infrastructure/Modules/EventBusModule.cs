using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac;
using NEventStore.Dispatcher;
using MediatR;

namespace SciVacancies.WebApp.Infrastructure.Modules
{
    public class EventBusModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>().As<IMediator>().SingleInstance();
            //builder.RegisterInstance<SingleInstanceFactory>();
        }
    }
}
