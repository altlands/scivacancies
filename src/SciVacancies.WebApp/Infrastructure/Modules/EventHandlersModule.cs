//using SciVacancies.Services.Logging;

using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
//using Autofac.Extras.DynamicProxy;
using FluentValidation;
using MediatR;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.EventHandlers;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Infrastructure.Saga;
using Module = Autofac.Module;
//using Castle.Core.Internal;

namespace SciVacancies.WebApp.Infrastructure
{

    public class EventHandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(EventBase).Assembly)
                .SingleInstance();
            builder.RegisterAssemblyTypes(typeof(CommandBase).Assembly)
                .AsClosedTypesOf(typeof(IRequest<>))
                .SingleInstance();

            builder.RegisterAssemblyTypes(new Assembly[]
            {
                Assembly.Load("SciVacancies.WebApp"),
                Assembly.Load("SciVacancies.ReadModel"),
                Assembly.Load("SciVacancies.ReadModel.Notifications"),
                Assembly.Load("SciVacancies.ReadModel.ElasticSearchModel"),
                Assembly.Load("SciVacancies.Services"),
                Assembly.Load("SciVacancies.SmtpNotifications")
            })
                .AsClosedTypesOf(typeof(INotificationHandler<>))
                //.EnableInterfaceInterceptors()
                //.InterceptedBy(typeof(CallLogger))
                .AsImplementedInterfaces()
                ;

            builder.RegisterAssemblyTypes(new Assembly[]
            {
                Assembly.Load("SciVacancies.WebApp")
            })
                .AsClosedTypesOf(typeof(IValidator<>))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes()
                .Where(t => !t.Name.StartsWith("ValidatorHandler"))
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                //.EnableInterfaceInterceptors()
                //.InterceptedBy(typeof(CallLogger))
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
              .SingleInstance()
            //.EnableInterfaceInterceptors(new Castle.DynamicProxy.ProxyGenerationOptions { })
            //.InterceptedBy(typeof(CallLogger))
            ;

            builder.RegisterGenericDecorator(
                typeof(ValidatorHandler<,>),
                typeof(IRequestHandler<,>),
                fromKey: "handler-implementor");
        }
    }
}
