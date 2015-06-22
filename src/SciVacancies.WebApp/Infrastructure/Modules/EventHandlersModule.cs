using Autofac;
using MediatR;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.EventHandlers;

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
