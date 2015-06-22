using Autofac;
using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.Domain.Aggregates.Services;

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