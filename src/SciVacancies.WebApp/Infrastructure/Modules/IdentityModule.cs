using Autofac;
using Microsoft.AspNet.Identity;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.Infrastructure.WebAuthorize;

namespace SciVacancies.WebApp.Infrastructure
{
    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostgresSciVacUserDbContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<PostgresSciVacUserDbContext>().As<SciVacUserDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<SciVacUserStore>().As<IUserStore<SciVacUser>>().InstancePerLifetimeScope();
            builder.RegisterType<SciVacUserManager>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<AuthorizeService>().As<IAuthorizeService>().InstancePerLifetimeScope();
            builder.RegisterType<RecoveryPasswordService>().As<IRecoveryPasswordService>().InstancePerLifetimeScope();
        }
    }
}