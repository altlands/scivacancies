using SciVacancies.Domain.Events;
using SciVacancies.WebApp.Infrastructure;

using System;
using System.Collections.Generic;

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql.SqlDialects;

using CommonDomain;
using CommonDomain.Core;
using CommonDomain.Persistence;
using CommonDomain.Persistence.EventStore;

using MediatR;
using Autofac;
using Autofac.Dnx;
using Autofac.Features.Variance;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SciVacancies.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Setup configuration sources.
            Configuration = new Configuration()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSubKey("AppSettings"));
            services.Configure<DbSettings>(Configuration.GetSubKey("Data"));

            services.ConfigureCookieAuthentication(options =>
            {
                options.AutomaticAuthentication = true;
            });
            services.AddMvc();

            var builder = new ContainerBuilder();

            ConfigureContainer(builder);

            builder.Populate(services);
            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            app.UseCookieAuthentication(options =>
            {
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

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new EventStoreModule(Configuration));
            builder.RegisterModule(new EventBusModule());
            builder.RegisterModule(new EventHandlersModule());
            builder.RegisterModule(new ReadModelModule(Configuration));
            builder.RegisterModule(new ServicesModule());
            builder.RegisterModule(new IdentityModule());

            //ConfigureMediatr(builder);
            builder.RegisterType<Fart>().As<IFart>();


            //builder.RegisterType<PostgresSciVacUserDbContext>().AsSelf().InstancePerRequest();
            //builder.RegisterType<SciVacUserStore>().As<IUserStore<SciVacUser>>().InstancePerRequest();
            //builder.RegisterType<SciVacUserManager>().AsSelf().InstancePerRequest();

            //builder.RegisterType<PostgresSciVacUserDbContext>().AsSelf().InstancePerRequest();
            //builder.RegisterType<SciVacUserStore>().As<IUserStore<SciVacUser>>();
            //builder.RegisterType<SciVacUserManager>().AsSelf();

            //builder.RegisterType<SciVacSignInManager>().AsSelf().InstancePerRequest();
            //builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            //builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();



        }

        public void ConfigureMediatr(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(EventBase).Assembly).AsImplementedInterfaces();

            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });
        }
    }
}
