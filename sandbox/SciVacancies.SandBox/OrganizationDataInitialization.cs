using Autofac;
using CommonDomain.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.Framework.ConfigurationModel;
using SciVacancies.WebApp;
using SciVacancies.WebApp.Infrastructure;
using SciVacancies.WebApp.Infrastructure.Identity;
using Xunit;

namespace SciVacancies.SandBox
{
    public class OrganizationDataInitialization
    {
        public IConfiguration Configuration { get; set; }
        public IContainer Container { get; set; }
        public SciVacUserManager UserManager => Container.Resolve<SciVacUserManager>();
        public IRepository Repository => Container.Resolve<IRepository>();
        public ILifetimeScope Scope { get; set; }

        public OrganizationDataInitialization()
        {
            var config = new Configuration();
            config.AddJsonFile("config.json");
            Configuration = config;            
            var cr = new CompositionRoot(config);
            Container = cr.Container.Value;
            Scope = Container.BeginLifetimeScope();
        }
       
        public void AddUser(SciVacUser user)
        {            
            var result = UserManager.Create(user);
        }

        [Fact]
        public void InitializeOrganizations()
        {
            var appUserManager = Container.Resolve<SciVacUserManager>();

        }
    }
}
