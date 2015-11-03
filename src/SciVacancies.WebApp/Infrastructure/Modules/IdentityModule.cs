//using SciVacancies.Services.Logging;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.Infrastructure.WebAuthorize;

using Microsoft.AspNet.Identity;

using Autofac;
//using Autofac.Extras.DynamicProxy;

namespace SciVacancies.WebApp.Infrastructure
{
    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostgresSciVacUserDbContext>()
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterType<PostgresSciVacUserDbContext>()
                .As<SciVacUserDbContext>()
                .InstancePerLifetimeScope();
            builder.RegisterType<SciVacUserStore>()
                .As<IUserStore<SciVacUser>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<SciVacUserManager>()
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterType<AuthorizeService>()
                .As<IAuthorizeService>()
                .InstancePerLifetimeScope();
        }
    }
}