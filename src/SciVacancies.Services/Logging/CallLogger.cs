//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//using Castle.DynamicProxy;

//namespace SciVacancies.Services.Logging
//{
//    public class CallLogger : Castle.Core.Internal.InternalsVisible, IInterceptor
//    {
//        public void Intercept(IInvocation invocation)
//        {
//            TracingEventSource.LogTracing.Invoke(
//                invocation.TargetType != null ? invocation.TargetType.ToString() : "TargetType:NULL",
//                invocation.Method != null ? invocation.Method.Name : "MethodName:NULL",
//                string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())
//                );
//            try
//            {
//                invocation.Proceed();
//            }
//            catch (Exception e)
//            {
//                TracingEventSource.LogTracing.Error(e.Message);
//            }
//            TracingEventSource.LogTracing.Finish(
//                invocation.TargetType != null ? invocation.TargetType.ToString() : "TargetType:NULL",
//                invocation.Method != null ? invocation.Method.Name : "MethodName:NULL",
//                invocation.ReturnValue != null ? invocation.ReturnValue.ToString() : "ReturnValue:NULL"
//                );
//        }
//    }
//}
