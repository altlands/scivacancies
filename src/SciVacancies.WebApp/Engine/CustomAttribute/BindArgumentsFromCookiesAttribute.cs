using System;
using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Engine.CustomAttribute
{
    public class BindArgumentFromCookiesAttribute : ActionFilterAttribute
    {
        private readonly string _key;
        private readonly string _arguName;

        public BindArgumentFromCookiesAttribute(string keys, string arguName)
        {
            _key= keys;
            _arguName = arguName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
                if(context.HttpContext.Request.Cookies.ContainsKey(_key))
                    context.ActionArguments[_arguName] = Guid.Parse(context.HttpContext.Request.Cookies.Get(_key));

            base.OnActionExecuting(context);
        }
    }
}