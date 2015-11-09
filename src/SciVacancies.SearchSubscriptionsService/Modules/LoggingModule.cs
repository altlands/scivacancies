using SciVacancies.Services.Logging;

using System;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Configuration;

using Autofac;
using Serilog;

namespace SciVacancies.SearchSubscriptionsService.Modules
{
    public class LoggingModule : Module
    {
        public IConfiguration Configuration { get; set; }

        public LoggingModule(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            ILoggerFactory loggerFactory = new LoggerFactory();

            var serilogLogger = new LoggerConfiguration()
            .WriteTo
            .RollingFile(
                Configuration["LogSettings:FileName"],
                (Serilog.Events.LogEventLevel)Enum.Parse(typeof(Serilog.Events.LogEventLevel), Configuration["LogSettings:LogLevel"], true),
                Configuration["LogSettings:TimeStampPattern"],
                null,
                long.Parse(Configuration["LogSettings:FileSizeBytes"]),
                int.Parse(Configuration["LogSettings:FileCountLimit"])
            )
            .WriteTo
            .ColoredConsole()
            .MinimumLevel.Information()
            .CreateLogger();

            loggerFactory.MinimumLevel = (LogLevel)Enum.Parse(typeof(LogLevel), Configuration["LogSettings:LogLevel"], true);
            loggerFactory.AddSerilog(serilogLogger);

            builder.Register(c => loggerFactory)
                .As<ILoggerFactory>()
                .SingleInstance();

            builder.Register(c => new CallLogger(c.Resolve<ILoggerFactory>()))
                .InstancePerDependency();
        }
    }
}
