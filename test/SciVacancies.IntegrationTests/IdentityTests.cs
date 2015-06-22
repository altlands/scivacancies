using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Framework.ConfigurationModel;
using SciVacancies.WebApp.Infrastructure.Identity;
using Xunit;

namespace SciVacancies.IntegrationTests
{
    public class IdentityTests
    {
        public IConfiguration Config { get; set; }
        public IdentityTests()
        {
            var config = new Configuration();
            config.AddJsonFile("config.json");
            Config = config;
        }

        [Fact]
        public void AddUserToMssql()
        {
            var dbContext = new SciVacUserDbContext(Config.Get("DB:MssqlIdentity"));
            var appUserManager = new SciVacUserManager(new UserStore<SciVacUser>(dbContext));
            var result = appUserManager.Create(new SciVacUser()
            {
                UserName = Guid.NewGuid().ToString("N")
            });

            Assert.True(result.Succeeded);
        }

        [Fact]
        public void AddUserToPostgres()
        {
            var dbContext = new PostgresSciVacUserDbContext(Config.Get("DB:PostgresIdentity"));
            var appUserManager = new SciVacUserManager(new UserStore<SciVacUser>(dbContext));
            var result = appUserManager.Create(new SciVacUser()
            {
                UserName = Guid.NewGuid().ToString("N")
            });

            Assert.True(result.Succeeded);
        }
    }
}
