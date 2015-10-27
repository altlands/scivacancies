using Autofac;
using SciVacancies.Services;
using SciVacancies.Services.Email;
using SciVacancies.SmtpNotifications.SmtpNotificators;

namespace SciVacancies.WebApp.Infrastructure
{
    public class SmtpNotificationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();
            builder.RegisterType<SmtpNotificatorAccountService>().As<ISmtpNotificatorAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<SmtpNotificatorVacancyService>().As<ISmtpNotificatorVacancyService>().InstancePerLifetimeScope();
            builder.RegisterType<SmtpNotificatorSearchSubscriptionService>().As<ISmtpNotificatorSearchSubscriptionService>().InstancePerLifetimeScope();
        }
    }
}