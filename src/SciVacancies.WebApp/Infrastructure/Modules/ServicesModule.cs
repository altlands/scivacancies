using SciVacancies.Domain.Interfaces;
using SciVacancies.Domain.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac;

namespace SciVacancies.WebApp.Infrastructure
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OrganizationService>().As<IOrganizationService>().SingleInstance();
            builder.RegisterType<ResearcherService>().As<IResearcherService>().SingleInstance();
        }
    }
}