using System;
using System.Collections;
using System.Linq;
using Autofac;
using Autofac.Framework.DependencyInjection;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using SciVacancies.Services.Quartz;
using SciVacancies.WebApp.Infrastructure;
using Microsoft.Dnx.Runtime;
using Microsoft.AspNet.StaticFiles;
using Microsoft.AspNet.Session;
using System.Globalization;
using Quartz.Spi;

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
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{devEnv}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = configurationBuilder.Build();
        }

        public IConfiguration Configuration { get; set; }

        public IContainer Container { get; set; }

        // This method gets called by the runtime.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<DbSettings>(Configuration.GetSection("Data"));
            services.Configure<OAuthSettings>(Configuration.GetSection("OAuthSettings"));
            services.Configure<QuartzSettings>(Configuration.GetSection("QuartzSettings"));
            services.Configure<ApiSettings>(Configuration.GetSection("ApiSettings"));
            services.Configure<ElasticSettings>(Configuration.GetSection("ElasticSettings"));
            services.Configure<AttachmentSettings>(Configuration.GetSection("AttachmentSettings"));
            services.Configure<SiteFileSettings>(Configuration.GetSection("SiteFileSettings"));
            services.Configure<VacancyLifeCycleSettings>(Configuration.GetSection("VacancyLifeCycleSettings"));
            services.Configure<CaptchaSettings>(Configuration.GetSection("CaptchaSettings"));
            services.Configure<SagaSettings>(Configuration.GetSection("SagaSettings"));

            services.AddCookieAuthentication(options =>
            {
                options.AutomaticAuthentication = false;
            });

            services.AddMvc();


            //TODO -  remove
            services.AddSingleton(c => Configuration);


            services.AddCaching();

            services.AddSession(o => { o.IdleTimeout = TimeSpan.FromSeconds(10); });

            var builder = new ContainerBuilder();

            ConfigureContainer(builder);

            builder.Populate(services);

            Container = builder.Build();

            return Container.Resolve<IServiceProvider>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
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

            app.UseSession();
            // Configure the HTTP request pipeline.          

            // Add the console logger.
            loggerfactory.AddConsole();

            // Add the following to the request pipeline only in development environment.
            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePagesWithReExecute("/StatusCodes/StatusCode{0}");
            //app.UseMvcWithDefaultRoute();


            app.UseIISPlatformHandler();

            // Add static files to the request pipeline.
            app.UseStaticFiles();

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

            var schedulerService = new QuartzService(Configuration, Container.Resolve<IJobFactory>());
            schedulerService.StartScheduler();
        }
    }

}
