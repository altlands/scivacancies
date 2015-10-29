using System;
using System.Diagnostics.Tracing;
using AltLanDS.Common.Events;
using EventSourceProxy;

namespace SciVacancies.Services.Logging
{
    public class LogEvents
    {
        private static readonly Lazy<ILogEvents> _log = new Lazy<ILogEvents>(
           () =>
           {
               TraceParameterProvider.Default.For<ILogEvents>().AddActivityIdContext();
               return EventSourceImplementer.GetEventSourceAs<ILogEvents>();
           });

        public static ILogEvents Log
        {
            get
            {
                return _log.Value;
            }
        }

        public static EventSource LogEventSource
        {
            get
            {
                return _log.Value as EventSource;
            }
        }
    }
}
