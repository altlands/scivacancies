using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using FluentValidation;
using MediatR;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.EventHandlers;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Infrastructure.Saga;
using Module = Autofac.Module;
using OrganizationEventsHandler = SciVacancies.ReadModel.ElasticSearchModel.EventHandlers.OrganizationEventsHandler;

namespace SciVacancies.WebApp.Infrastructure
{
    public class EventHandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterAssemblyTypes(typeof(VacancyApplicationEventsHandler).Assembly).AsClosedTypesOf(typeof(INotificationHandler<>)).AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(typeof(OrganizationEventsHandler).Assembly).AsClosedTypesOf(typeof(INotificationHandler<>)).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(EventBase).Assembly).SingleInstance();
            builder.RegisterAssemblyTypes(typeof(CommandBase).Assembly).AsClosedTypesOf(typeof(IRequest<>)).SingleInstance();

            builder.RegisterAssemblyTypes(new Assembly[]
            {
                Assembly.Load("SciVacancies.ReadModel"),
                Assembly.Load("SciVacancies.ReadModelNotifications"),
                Assembly.Load("SciVacancies.ReadModel.ElasticSearchModel"),
                Assembly.Load("SciVacancies.SmtpNotifications"),
                Assembly.Load("SciVacancies.SmtpNotificationsHandlers")
            })
                .AsClosedTypesOf(typeof(INotificationHandler<>))
                .AsImplementedInterfaces();

            //builder.RegisterAssemblyTypes(new Assembly[]
            //{
            //    Assembly.Load("SciVacancies.WebApp")
            //})
            //    .AsClosedTypesOf(typeof(ISagaMessageHandler<>))
            //    .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(new Assembly[]
            {
                Assembly.Load("SciVacancies.WebApp")
            })
                .AsClosedTypesOf(typeof(IValidator<>))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes()
                .Where(t => !t.Name.StartsWith("ValidatorHandler"))
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces();
            //.Keyed("implementation", typeof(IRequestHandler<,>));


            builder.RegisterAssemblyTypes(new Assembly[]
            {
                Assembly.Load("SciVacancies.WebApp")
            })
              .As(t => t.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                    .Select(i => new KeyedService("handler-implementor", i))
                    .Cast<Service>())
              .SingleInstance();

            // builder.RegisterType<CreatePositionCommandHandler>().Named<IRequestHandler<CreatePositionCommand, Guid>>("implementation");

            builder.RegisterGenericDecorator(
                typeof(ValidatorHandler<,>),
                typeof(IRequestHandler<,>),
                fromKey: "handler-implementor");

            //builder.RegisterAssemblyTypes(typeof(CreateOrganizationCommandHandler).Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>)).SingleInstance();
        }
    }
}
