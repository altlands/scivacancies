using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.Domain.Aggregates.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac;
using Microsoft.AspNet.Identity;

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
        }
    }
}