using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Handlers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac;
using MediatR;

namespace SciVacancies.WebApp.Infrastructure
{
    public class EventHandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(EventBaseHandler<>).Assembly).AsClosedTypesOf(typeof(INotificationHandler<>)).SingleInstance();
            builder.RegisterAssemblyTypes(typeof(EventBase).Assembly).SingleInstance();
        }
    }
}
