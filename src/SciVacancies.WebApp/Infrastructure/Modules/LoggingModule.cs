using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SciVacancies.Services.Logging;
using Serilog;
using Serilog.Events;

namespace SciVacancies.WebApp.Infrastructure.Modules
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new CallLogger(c.Resolve<ILoggerFactory>()))
                .InstancePerDependency();
        }

        ///// <summary>
        ///// все логи пишутся в один файл
        ///// </summary>
        ///// <param name="configuration"></param>
        ///// <returns></returns>
        //internal static LoggerConfiguration LoggerConfigurationSingleFile(IConfiguration configuration)
        //{
        //    return new LoggerConfiguration()
        //        .WriteTo
        //        .RollingFile(
        //            configuration["LogSettings:FileName"],
        //            (LogEventLevel)Enum.Parse(typeof(LogEventLevel), configuration["LogSettings:LogLevel"], true),
        //            configuration["LogSettings:TimeStampPattern"],
        //            null,
        //            long.Parse(configuration["LogSettings:FileSizeBytes"]),
        //            int.Parse(configuration["LogSettings:FileCountLimit"])
        //        )
        //        .WriteTo.ColoredConsole()
        //        .MinimumLevel.Information();
        //}

        /// <summary>
        /// логи каждого уровня пишутся в отдельные файлы
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        internal static LoggerConfiguration LoggerConfiguration(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .WriteTo.Logger(
                    lc => lc
                        .Filter
                        .ByIncludingOnly(evt => evt.Level == LogEventLevel.Verbose)
                        .WriteTo.RollingFile(
                            pathFormat: "logs" + Path.DirectorySeparatorChar + configuration["LogSettings:FileNameVerbose"],
                            outputTemplate: configuration["LogSettings:TimeStampPattern"],
                            fileSizeLimitBytes: long.Parse(configuration["LogSettings:FileSizeBytes"]),
                            retainedFileCountLimit: int.Parse(configuration["LogSettings:FileCountLimit"])
                        )
                )
                .WriteTo.Logger(
                    lc => lc
                        .Filter
                        .ByIncludingOnly(evt => evt.Level == LogEventLevel.Debug)
                        .WriteTo.RollingFile(
                            pathFormat: "logs" + Path.DirectorySeparatorChar + configuration["LogSettings:FileNameDebug"],
                            outputTemplate: configuration["LogSettings:TimeStampPattern"],
                            fileSizeLimitBytes: long.Parse(configuration["LogSettings:FileSizeBytes"]),
                            retainedFileCountLimit: int.Parse(configuration["LogSettings:FileCountLimit"])
                        )
                )
                .WriteTo.Logger(
                    lc => lc
                        .Filter
                        .ByIncludingOnly(evt => evt.Level == LogEventLevel.Information)
                        .WriteTo.RollingFile(
                            pathFormat: "logs" + Path.DirectorySeparatorChar + configuration["LogSettings:FileNameInformation"],
                            outputTemplate: configuration["LogSettings:TimeStampPattern"],
                            fileSizeLimitBytes: long.Parse(configuration["LogSettings:FileSizeBytes"]),
                            retainedFileCountLimit: int.Parse(configuration["LogSettings:FileCountLimit"])
                        )
                )
                .WriteTo.Logger(
                    lc => lc
                        .Filter
                        .ByIncludingOnly(evt => evt.Level == LogEventLevel.Warning)
                        .WriteTo.RollingFile(
                            pathFormat: "logs" + Path.DirectorySeparatorChar + configuration["LogSettings:FileNameWarning"],
                            outputTemplate: configuration["LogSettings:TimeStampPattern"],
                            fileSizeLimitBytes: long.Parse(configuration["LogSettings:FileSizeBytes"]),
                            retainedFileCountLimit: int.Parse(configuration["LogSettings:FileCountLimit"])
                        )
                )
                .WriteTo.Logger(
                    lc => lc
                        .Filter
                        .ByIncludingOnly(evt => evt.Level == LogEventLevel.Error)
                        .WriteTo.RollingFile(
                            pathFormat: "logs" + Path.DirectorySeparatorChar + configuration["LogSettings:FileNameError"],
                            outputTemplate: configuration["LogSettings:TimeStampPattern"],
                            fileSizeLimitBytes: long.Parse(configuration["LogSettings:FileSizeBytes"]),
                            retainedFileCountLimit: int.Parse(configuration["LogSettings:FileCountLimit"])
                        )
                )
                .WriteTo.Logger(
                    lc => lc
                        .Filter
                        .ByIncludingOnly(evt => evt.Level == LogEventLevel.Fatal)
                        .WriteTo.RollingFile(
                            pathFormat: "logs" + Path.DirectorySeparatorChar + configuration["LogSettings:FileNameFatal"],
                            outputTemplate: configuration["LogSettings:TimeStampPattern"],
                            fileSizeLimitBytes: long.Parse(configuration["LogSettings:FileSizeBytes"]),
                            retainedFileCountLimit: int.Parse(configuration["LogSettings:FileCountLimit"])
                        )
                )
                .WriteTo.ColoredConsole();
        }
    }
}
