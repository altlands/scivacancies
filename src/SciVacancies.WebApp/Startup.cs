using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Web.ModelBinding;
using Autofac;
using Autofac.Dnx;
using Autofac.Features.Variance;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using SciVacancies.Services.Quartz;
using SciVacancies.WebApp.Infrastructure;

namespace SciVacancies.WebApp
{
    public class Startup
    {
        private readonly string devEnv;

        public Startup(IHostingEnvironment env)
        {
            var vars = Environment.GetEnvironmentVariables();
            devEnv = (string)vars.Cast<DictionaryEntry>().FirstOrDefault(e => e.Key.Equals("dev_env")).Value;
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
            services.Configure<OAuthSettings>(Configuration.GetSubKey("OAuthSettings"));
            services.Configure<QuartzSettings>(Configuration.GetSubKey("QuartzSettings"));
            services.Configure<ApiSettings>(Configuration.GetSubKey("ApiSettings"));
            services.Configure<ElasticSettings>(Configuration.GetSubKey("ElasticSettings"));
            services.Configure<AttachmentSettings>(Configuration.GetSubKey("AttachmentSettings"));
            services.Configure<SiteFileSettings>(Configuration.GetSubKey("SiteFileSettings"));
            services.Configure<VacancyLifeCycleSettings>(Configuration.GetSubKey("VacancyLifeCycleSettings"));
            services.Configure<CaptchaSettings>(Configuration.GetSubKey("CaptchaSettings"));
            services.Configure<SagaSettings>(Configuration.GetSubKey("SagaSettings"));

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

            //ModelBinders.Binders.Add(typeof(DateTime), new SciVacancyModelBinder());
        }
        public void SchedulerServiceInitialize(ISchedulerService schedulerService)
        {
            schedulerService.StartScheduler();
        }
    }

    //public class SciVacancyModelBinder : IModelBinder
    //{
    //    public bool BindModel(ModelBindingExecutionContext modelBindingExecutionContext, ModelBindingContext bindingContext)
    //    {
    //        string theDate = bindingContext.Context.Request.QueryString["InCommitteeDate"];
    //        //       DateTime dt = new DateTime();
    //        //       bool success = DateTime.TryParse(theDate, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out dt);
    //        //       if (success)
    //        //       {
    //        //           return new ModelBinderResult(dt);
    //        //       }
    //        //       else
    //        //       {
    //        //           // Return an appropriate default
    //        //       }
    //    }
    //}

}
