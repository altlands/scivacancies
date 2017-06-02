using System;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SciVacancies.Services.Logging
{
    public class CallLogger : InternalsVisible, IInterceptor
    {
        private readonly ILogger _logger;

        public CallLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CallLogger>();
        }
        public void Intercept(IInvocation invocation)
        {
            string invokeMessage = "Empty invocation message";
            try
            {
                invokeMessage = "Invoke :" + " Class = " + invocation.TargetType + ", Method = " + invocation.Method.Name + ", SerializedArguments = " + JsonConvert.SerializeObject(invocation.Arguments);
            }
            catch (Exception e)
            {
                _logger.LogError("ERROR while composing Invoke message");
                CurrentLogger(e);
            }

            _logger.LogInformation(invokeMessage);
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                _logger.LogError("ERROR while invocation.proceed().");
                CurrentLogger(e);
                if (e.InnerException != null)
                {
                    _logger.LogError("ERROR while invocation.proceed().InnerException:");
                    CurrentLogger(e.InnerException);
                }
            }
            string finishMessage = "Empty finish message";

            try
            {
                string output = invocation.ReturnValue?.GetType().ToString() ?? "NULL";
                finishMessage = "Finish :" + " Class = " + invocation.TargetType + ", Method = " + invocation.Method.Name + ", Output = " + output;
            }
            catch (Exception e)
            {
                _logger.LogError("ERROR while composing Finish message");
                CurrentLogger(e);
            }

            _logger.LogInformation(finishMessage);
        }

        private void CurrentLogger(Exception e)
        {
            var errorEpipeBrokenPipe = "Error -32 EPIPE broken pipe";
            if (e.Message.Contains(errorEpipeBrokenPipe) || e.StackTrace.Contains(errorEpipeBrokenPipe))
            {
                //_logger.Log
            }
            else
            {
                _logger.LogError(e.Message, e);
            }
        }
    }
}
