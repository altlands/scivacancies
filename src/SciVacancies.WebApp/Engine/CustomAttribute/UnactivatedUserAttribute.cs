using System;
using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Controllers
{
    /// <summary>
    /// если пользователь не активирован, то отправить его на страницу запроса активации
    /// </summary>
    public class UnactivatedUserAttribute : ActionFilterAttribute
    {
        protected string Key;
        protected string ArguName;
        protected Type ArgumentType;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //если пользователь еще не активирован, то он не имеет роли Исследователя
            if (!((Controller)context.Controller).User.IsInRole(ConstTerms.RequireRoleResearcher))
            {
                context.HttpContext.Response.Redirect("/account/activationrequest");
                //return RedirectToAction("activationrequest", "account");
            }
        }
    }

}