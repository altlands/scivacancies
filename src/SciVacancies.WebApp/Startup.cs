using System;
using System.Collections;
using System.Linq;
using Autofac;
using Autofac.Dnx;
using Autofac.Features.Variance;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.WebEncoders;
using SciVacancies.WebApp.Infrastructure;

namespace SciVacancies.WebApp
{
	public class Startup
	{
		private readonly string devEnv;

		public Startup(IHostingEnvironment env)
		{
			var vars = Environment.GetEnvironmentVariables();
			devEnv = (string) vars.Cast<DictionaryEntry>().FirstOrDefault(e => e.Key.Equals("dev_env")).Value;
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
			services.Configure<ApiSettings>(Configuration.GetSubKey("ApiSettings"));

			services.ConfigureCookieAuthentication(options =>
			{
			    options.LoginPath = new PathString("/account/login");
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
			builder.RegisterModule(new IdentityModule());
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
				
                //A-scenatio
				app.UseErrorHandler("/Home/Error");
			}

			////B-scenatio
			//// Configure the error handler to show an error page.
   //         app.UseErrorHandler(errorApp =>
   //   						{
   //   							// Normally you'd use MVC or similar to render a nice page.
   //             errorApp.Run(async context =>
   //     		{
   //     			context.Response.StatusCode = 500;
   //     			context.Response.ContentType = "text/html";
   //     			await context.Response.WriteAsync("<html>\r\n");
   //     				await context.Response.WriteAsync("<head>\r\n");
   //     					await context.Response.WriteAsync("<meta charset=\"utf-8\">\r\n");
   //     				await context.Response.WriteAsync("<head>\r\n");
   //				    await context.Response.WriteAsync("<body>\r\n");
   //                         await context.Response.WriteAsync("Мы сожалеем но возникла ошибка при работе приложения.<br>\r\n");

   //                         var error = context.GetFeature<IErrorHandlerFeature>();
   //                         if (error != null)
   //                         {
   //                             // This error would not normally be exposed to the client
   //                             await context.Response.WriteAsync("<br>Описание ошибки: " + HtmlEncoder.Default.HtmlEncode(error.Error.Message) + "<br>\r\n");
   //                         }
   //                         await context.Response.WriteAsync("<br><a href=\"/\">Главная страница</a><br>\r\n");
   //                     await context.Response.WriteAsync("</body>\r\n");
   //                 await context.Response.WriteAsync("</html>\r\n");
   //                 await context.Response.WriteAsync(new string(' ', 512)); // Padding for IE
   //             });
   //         });
   //         // We could also configure it to re-execute the request on the normal pipeline with a different path.
   //          app.UseErrorHandler("/error.html");
   //         // The broken section of our application.
   //         app.Map("/throw", throwApp =>
   //         {
   //             throwApp.Run(context => { throw new Exception("Application Exception"); });
   //         });

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

            MappingConfiguration.Initialize();

            //SearchSubscriptionService.Initialize(Configuration);
        }
    }
}
