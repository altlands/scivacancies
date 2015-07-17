using System;
using System.Collections;
using System.Linq;
using Autofac;
using Autofac.Dnx;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
//using SciVacancies.ApplicationInfrastructure;
using SciVacancies.WebApp;

namespace SciVacancies.SandBox
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var vars = Environment.GetEnvironmentVariables();
            var devEnv = vars.Cast<DictionaryEntry>().FirstOrDefault(e => e.Key.Equals("dev_env")).Value;
            // Setup configuration sources.
            Configuration = new Configuration()
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{devEnv}.json", optional: true)
                .AddEnvironmentVariables();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSubKey("AppSettings"));
            services.Configure<DbSettings>(Configuration.GetSubKey("Data"));                     

            var builder = new ContainerBuilder();

            ConfigureContainer(builder);

            builder.Populate(services);
            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {           
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {                                 
        }
    }
}
