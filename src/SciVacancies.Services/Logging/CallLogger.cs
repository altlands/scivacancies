using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Framework.Logging;

using Castle.DynamicProxy;
using Newtonsoft.Json;

namespace SciVacancies.Services.Logging
{
    public class CallLogger : Castle.Core.Internal.InternalsVisible, IInterceptor
    {
        private readonly ILogger _logger;

        public CallLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CallLogger>();
            _logger.LogInformation("Constructing CallLogger");
        }
        public void Intercept(IInvocation invocation)
        {
            //string invokeMessage = "Invoke :" + " Class = " + invocation.TargetType.ToString() + ", Method = " + invocation.Method.Name + ", Args = " + string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray());
            //string invokeMessage = "Invoke :" + " Class = " + invocation.TargetType.ToString() + ", Method = " + invocation.Method.Name;
            string invokeMessage = "Invoke :" + " Class = " + invocation.TargetType.ToString() + ", Method = " + invocation.Method.Name + ", SerializedArguments = "+ JsonConvert.SerializeObject(invocation.Arguments);

            _logger.LogInformation(invokeMessage);
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                _logger.LogError("ERROR");
                _logger.LogError(e.Message, e);
            }
            string finishMessage = "Finish :" + " Class = " + invocation.TargetType.ToString() + ", Method = " + invocation.Method.Name + ", Output = " + invocation.ReturnValue.GetType().ToString();
            //string finishMessage = "Finish :" + " Class = " + invocation.TargetType.ToString() + ", Method = " + invocation.Method.Name;

            _logger.LogInformation(finishMessage);
        }
    }
}
