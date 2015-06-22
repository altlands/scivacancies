using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.Framework.ConfigurationModel;
using SciVacancies.WebApp.Infrastructure;
using SciVacancies.WebApp.Infrastructure.Identity;
using Xunit;

namespace SciVacancies.SandBox
{
    public class UserDataInitialization
    {
        public IConfiguration Configuration { get; set; }
        public IContainer Container { get; set; }
        public SciVacUserManager UserManager => Container.Resolve<SciVacUserManager>();
        public ILifetimeScope Scope { get; set; }

        public UserDataInitialization()
        {
            var config = new Configuration();
            config.AddJsonFile("config.json");
            Configuration = config;
            var cr = new CompositionRoot(config);
            Container = cr.Container.Value;
            Scope = Container.BeginLifetimeScope();
        }
       
        public void CreateUserIfNotExists(SciVacUser user)
        {
            var storedUser = UserManager.FindByName(user.UserName);
            if (storedUser == null)
            {
                UserManager.Create(user);
            }            
        }

        [Fact]
        public void InitializeUsers()
        {            

        }
    }
}
