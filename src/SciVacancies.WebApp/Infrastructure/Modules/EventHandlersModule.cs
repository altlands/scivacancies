using Autofac;
using MediatR;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.EventHandlers;
using SciVacancies.WebApp.Commands;

namespace SciVacancies.WebApp.Infrastructure
{
    public class EventHandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(EventBaseHandler<>).Assembly).AsClosedTypesOf(typeof(INotificationHandler<>)).SingleInstance();
            builder.RegisterAssemblyTypes(typeof(EventBase).Assembly).SingleInstance();

            builder.RegisterAssemblyTypes(typeof(CommandBase).Assembly).AsClosedTypesOf(typeof(IRequest<>)).SingleInstance();
            builder.RegisterAssemblyTypes(typeof(CreateOrganizationCommandHandler).Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>)).SingleInstance();
        }
    }
}
