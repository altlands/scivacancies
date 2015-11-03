using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace SciVacancies.Services.Logging
{
    [EventSource(Name = "TracingEventSource")]
    public sealed class TracingEventSource : EventSource
    {
        public static TracingEventSource LogTracing = new TracingEventSource();

        [Event(1, Message = "Invoke : ", Level = EventLevel.Verbose)]
        public void Invoke(string _class, string method, string args)
        {
            this.WriteEvent(1, _class, method, args);
        }
        [Event(2, Message = "Finally : ", Level = EventLevel.Verbose)]
        public void Finish(string _class, string method, string output)
        {
            this.WriteEvent(2, _class, method, output);
        }
        [Event(3, Message = "Exception : ", Level = EventLevel.Verbose)]
        public void Error(string message)
        {
            this.WriteEvent(3, message);
        }
    }
}
