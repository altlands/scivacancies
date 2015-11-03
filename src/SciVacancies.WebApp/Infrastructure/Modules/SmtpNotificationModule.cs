//using SciVacancies.Services.Logging;
using SciVacancies.Services.Email;
using SciVacancies.SmtpNotifications.SmtpNotificators;

using Autofac;
//using Autofac.Extras.DynamicProxy;

namespace SciVacancies.WebApp.Infrastructure
{
    public class SmtpNotificationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailService>()
                .As<IEmailService>()
                .InstancePerLifetimeScope()
                //.EnableInterfaceInterceptors()
                //.InterceptedBy(typeof(CallLogger))
                ;
            builder.RegisterType<SmtpNotificatorAccountService>()
                .As<ISmtpNotificatorAccountService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<SmtpNotificatorVacancyService>()
                .As<ISmtpNotificatorVacancyService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<SmtpNotificatorSearchSubscriptionService>()
                .As<ISmtpNotificatorSearchSubscriptionService>()
                .InstancePerLifetimeScope();
        }
    }
}