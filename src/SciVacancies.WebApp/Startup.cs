using System;
using System.Collections;
using System.Linq;
using Autofac;
using Autofac.Framework.DependencyInjection;
using Autofac.Features.Variance;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using SciVacancies.Services.Quartz;
using SciVacancies.WebApp.Infrastructure;
using Microsoft.Framework.Runtime;
using Microsoft.AspNet.StaticFiles;

namespace SciVacancies.WebApp
{
    public class Startup
    {
        private readonly string devEnv;

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var vars = Environment.GetEnvironmentVariables();
            devEnv = (string)vars.Cast<DictionaryEntry>().FirstOrDefault(e => e.Key.Equals("dev_env")).Value;
            // Setup configuration sources.
            var configurationBuilder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{devEnv}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = configurationBuilder.Build();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetConfigurationSection("AppSettings"));
            services.Configure<DbSettings>(Configuration.GetConfigurationSection("Data"));
            services.Configure<OAuthSettings>(Configuration.GetConfigurationSection("OAuthSettings"));
            services.Configure<QuartzSettings>(Configuration.GetConfigurationSection("QuartzSettings"));
            services.Configure<ApiSettings>(Configuration.GetConfigurationSection("ApiSettings"));
            services.Configure<ElasticSettings>(Configuration.GetConfigurationSection("ElasticSettings"));
            services.Configure<AttachmentSettings>(Configuration.GetConfigurationSection("AttachmentSettings"));
            services.Configure<SiteFileSettings>(Configuration.GetConfigurationSection("SiteFileSettings"));
            services.Configure<VacancyLifeCycleSettings>(Configuration.GetConfigurationSection("VacancyLifeCycleSettings"));
            services.Configure<CaptchaSettings>(Configuration.GetConfigurationSection("CaptchaSettings"));
            services.Configure<SagaSettings>(Configuration.GetConfigurationSection("SagaSettings"));

            services.ConfigureCookieAuthentication(options =>
            {
                options.AutomaticAuthentication = false;
            });

            services.AddMvc();

            //TODO -  remove
            services.AddSingleton(c => Configuration);

            var builder = new ContainerBuilder();
            builder.RegisterSource(new ContravariantRegistrationSource());

            ConfigureContainer(builder);

            builder.Populate(services);
            var container = builder.Build();

            SchedulerServiceInitialize(container.Resolve<ISchedulerService>());

            return container.Resolve<IServiceProvider>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());

            builder.RegisterModule(new EventStoreModule(Configuration));
            builder.RegisterModule(new EventBusModule());
            builder.RegisterModule(new EventHandlersModule());
            builder.RegisterModule(new ReadModelModule(Configuration));
            builder.RegisterModule(new ServicesModule(Configuration));
            builder.RegisterModule(new QuartzModule());
            builder.RegisterModule(new IdentityModule());
            builder.RegisterModule(new SmtpNotificationModule());
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            app.UseCookieAuthentication(options =>
            {
                options.AutomaticAuthentication = true;
                options.AuthenticationScheme = DefaultAuthenticationTypes.ApplicationCookie;
            });
            //app.UseOpenIdConnectAuthentication();

            app.UseInMemorySession(configure: s => s.IdleTimeout = TimeSpan.FromMinutes(30));
            // Configure the HTTP request pipeline.          

            // Add the console logger.
            loggerfactory.AddConsole();

            // Add the following to the request pipeline only in development environment.
            if (env.IsEnvironment("Development"))
            {
                app.UseErrorPage(ErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            app.UseStatusCodePagesWithReExecute("/StatusCodes/StatusCode{0}");
            //app.UseMvcWithDefaultRoute();


            // Add static files to the request pipeline.
            app.UseMiddleware<StaticFileMiddleware>(new StaticFileOptions());

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");


                routes.MapRoute("captchafetch", "captcha/fetch",
               new
               {
                   controller = "AltLanDSCaptcha",
                   action = "Fetch",
                   //w = UrlParameter.Optional,
                   //h = UrlParameter.Optional
               });

                routes.MapRoute("captchaisvalid", "captcha/isvalid",
                    new
                    {
                        controller = "AltLanDSCaptcha",
                        action = "IsValid"
                    });

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });

            MappingConfiguration.Initialize();
        }

        public void SchedulerServiceInitialize(ISchedulerService schedulerService)
        {
            schedulerService.StartScheduler();
        }
    }

}
