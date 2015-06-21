using System;
using Microsoft.AspNet.Mvc;
using System.ComponentModel;

namespace SciVacancies.WebApp.Engine.CustomAttribute
{
    public class BindArgumentFromCookiesAttribute : ActionFilterAttribute
    {
        private readonly string _key;
        private readonly string _arguName;
        private readonly Type _argumentType;

        public BindArgumentFromCookiesAttribute(string keys, string arguName)
        {
            _key = keys;
            _arguName = arguName;
            _argumentType = typeof(Guid);
        }
        public BindArgumentFromCookiesAttribute(string keys, string arguName, Type argumentType)
        {
            _key= keys;
            _arguName = arguName;
            _argumentType = argumentType;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
                if(context.HttpContext.Request.Cookies.ContainsKey(_key))
                    context.ActionArguments[_arguName] = TypeDescriptor.GetConverter(_argumentType).ConvertFrom(context.HttpContext.Request.Cookies.Get(_key));

            base.OnActionExecuting(context);
        }
    }
}