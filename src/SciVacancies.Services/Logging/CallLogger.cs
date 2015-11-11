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
            string invokeMessage = "Empty invocation message";
            try
            {
                invokeMessage = "Invoke :" + " Class = " + invocation.TargetType.ToString() + ", Method = " + invocation.Method.Name + ", SerializedArguments = " + JsonConvert.SerializeObject(invocation.Arguments);
            }
            catch (Exception e)
            {
                _logger.LogError("ERROR while composing Invoke message");
                _logger.LogError(e.Message, e);
            }

            _logger.LogInformation(invokeMessage);
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                _logger.LogError("ERROR while invocation.proceed()");
                _logger.LogError(e.Message, e);
            }
            string finishMessage = "Empty finish message";
        
            try
            {
                string output = invocation.ReturnValue != null ? invocation.ReturnValue.GetType().ToString() : "NULL";
                finishMessage = "Finish :" + " Class = " + invocation.TargetType.ToString() + ", Method = " + invocation.Method.Name + ", Output = " + output;
            }
            catch(Exception e)
            {
                _logger.LogError("ERROR while composing Finish message");
                _logger.LogError(e.Message, e);
            }

            _logger.LogInformation(finishMessage);
        }
    }
}
